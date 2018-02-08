using OpsService.Common.ApiInvoker;
using OpsUtil;
using SyncCustomerCodeService;
using SyncCustomerCodeService.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace SyncCustomerCodeService.service
{
    public class SyncCustomerCodeService
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>  
        public static DJS.SDK.ILog iLog = null;

        #endregion

        #region 构造函数

        static SyncCustomerCodeService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
        }
        #endregion

        /// <summary>
        /// 官网更新租客CustomerCode
        /// </summary>
        /// <param name="connString"></param>
        public static void updateCustomerCode(string connString)
        {
            iLog.WriteLog("doUpdateCustomerCode Begin", 0);
            SqlConnection sqlCon = null;
            SqlTransaction sqlTransaction = null;
            String UnUpdateCustomerSQL = @"SELECT * FROM dbo.Renters WHERE Status=1 AND ISNULL(CustomerCode,'')='' AND ISNULL(Phone,'')<>'' AND LEN(Phone)=11 ";
            //如果数据太多，每次只执行5个
            try
            {
                //查询CustomerCode为空的renters
                sqlCon = new SqlConnection(connString);
                sqlCon.Open();
                DataTable dtUnUpdateCustomer = Util.ExecuteSqlSelect(sqlCon, UnUpdateCustomerSQL);
                iLog.WriteLog("Customer count: " + dtUnUpdateCustomer.Rows.Count, 0);
                //SupervisorWS.WebsiteService.VLDService service = new SupervisorWS.WebsiteService.VLDService();
                //http://webtest.52mf.com.cn/api/v2_soap/?wsdl
                //更新CustomerCode
                for (int j = 0; j * 5 < dtUnUpdateCustomer.Rows.Count; j++)
                {
                    StringBuilder updateSql = new StringBuilder();
                    StringBuilder unUpdateCustomerJson = new StringBuilder();
                    unUpdateCustomerJson.Append("{\"Customers\":[");
                    if (dtUnUpdateCustomer != null && dtUnUpdateCustomer.Rows.Count > 0)
                    {
                        StringBuilder strCustomers = new StringBuilder();
                        for (int i = j * 5; (i < (j + 1) * 5 && i < dtUnUpdateCustomer.Rows.Count); i++)
                        {
                            DataRow rowItemObj = dtUnUpdateCustomer.Rows[i];
                            strCustomers.Append("{\"CustomerCode\":\"" + GetString(rowItemObj["CustomerCode"]) + "\",");
                            strCustomers.Append("\"Source\":\"" + ((int)UtilEnum.ProjectSource.AMS).ToString("00") + "\",");
                            strCustomers.Append("\"Name\":\"" + GetString(rowItemObj["Name"]) + "\",");
                            strCustomers.Append("\"Phone\":\"" + GetString(rowItemObj["Phone"]) + "\",");
                            String sex = "2";
                            if (!String.IsNullOrWhiteSpace(GetString(rowItemObj["Male"])))
                            {
                                sex = (GetString(rowItemObj["Male"]) == "True") ? "1" : "0";
                            }
                            strCustomers.Append("\"Sex\":\"" + sex + "\",");
                            strCustomers.Append("\"BirthDate\":\"\",");
                            strCustomers.Append("\"Address\":\"" + GetString(rowItemObj["Address"]) + "\",");
                            strCustomers.Append("\"Career\":\"" + GetString(rowItemObj["Career"]) + "\",");
                            strCustomers.Append("\"OtherSid\":\"" + GetString(rowItemObj["ID"]) + "\"},");
                        }
                        if (strCustomers.Length > 0)
                        {
                            strCustomers = strCustomers.Remove((strCustomers.Length - 1), 1);
                            unUpdateCustomerJson.Append(strCustomers.ToString());
                        }
                    }
                    unUpdateCustomerJson.Append("]}");

                    object[] args = new object[1];
                    args[0] = unUpdateCustomerJson.ToString();
                    //string webServiceURL = System.Configuration.ConfigurationManager.AppSettings["OfficialWebServiceURL"];//获取官网WebService地址
                    String resultJson = WebServiceHandler.InvokeWebService(ConstUtility.OfficialWebServiceURL, "SyncCustomerInfo", args).ToString();
                    //String resultJson = service.SyncCustomerInfo(unUpdateCustomerJson.ToString());

                    iLog.WriteLog("input:   " + unUpdateCustomerJson.ToString(), 0);
                    dynamic customeData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultJson);
                    iLog.WriteLog("result:  " + resultJson, 0);

                    //错误信息不为空则记录错误日志
                    if (customeData.Error.ToString() != "[]")
                    {
                        String strError = "";
                        for (int i = 0; i < customeData.Error.Count; i++)
                        {
                            strError += "{OtherSid：" + customeData.Error[i].OtherSid + ";Type：" + customeData.Error[i].Type + ";Description：" + customeData.Error[i].Description + "};";
                        }
                        iLog.WriteLog(string.Format("调用官网接口SyncCustomerInfo失败：{0}", strError), 0);
                    }

                    List<CustomerResult> resultCustomers = null;
                    try
                    {
                        if (customeData.Data != null && customeData.Data.ToString() != "[]")
                            resultCustomers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerResult>>(customeData.Data.ToString());
                    }
                    catch
                    {
                        //iLog.WriteLog("doUpdateCustomerCode Error " + ex.Message, 1);
                        throw;
                    }

                    if (resultCustomers != null && resultCustomers.Count > 0)
                    {
                        foreach (CustomerResult item in resultCustomers)
                        {
                            updateSql.AppendLine(" UPDATE dbo.Renters SET CustomerCode = '" + item.CustomerCode + "' WHERE ID = '" + item.OtherSid + "'; ");
                        }
                    }
                    iLog.WriteLog("doUpdateCustomerCode Exec SQL:" + updateSql.ToString(), 0);
                    if (updateSql.Length > 0)
                    {
                        sqlTransaction = sqlCon.BeginTransaction();
                        Util.ExecuteSqlNoReturn(sqlCon, sqlTransaction, updateSql.ToString());
                        sqlTransaction.Commit();
                        sqlTransaction = null;
                    }
                }
            }
            catch
            {
                //iLog.WriteLog("doUpdateCustomerCode Error " + ex.Message,1);
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                    sqlTransaction = null;
                }
                throw;
            }
            finally
            {
                if (sqlCon != null)
                    sqlCon.Close();
            }
        }

        /// <summary>
        /// CRM更新租客表CustomerCode
        /// </summary>
        /// <param name="connString"></param>
        public static void updateRenterCustomerCodeCRM(string connString)
        {
            iLog.WriteLog("更新租客表,调用CRM接口SyncCustomerInfo开始...", 0);
            SqlConnection sqlCon = null;
            SqlTransaction sqlTransaction = null;
            String UnUpdateCustomerSQL = @"SELECT * FROM dbo.Renters WHERE Status=1 AND ISNULL(CustomerCode,'')='' AND ISNULL(Phone,'')<>'' AND LEN(Phone)=11 ";

            try
            {
                //查询CustomerCode为空的renters
                sqlCon = new SqlConnection(connString);
                sqlCon.Open();
                DataTable dtUnUpdateCustomer = Util.ExecuteSqlSelect(sqlCon, UnUpdateCustomerSQL);
                iLog.WriteLog("Customer count: " + dtUnUpdateCustomer.Rows.Count, 0);

                for (int j = 0; j < dtUnUpdateCustomer.Rows.Count; j++)
                {
                    StringBuilder updateSql = new StringBuilder();

                    DataRow rowItemObj = dtUnUpdateCustomer.Rows[j];
                    if (ConstUtility.GetCustomerCodeTimeOut != "0")
                    {
                        //添加对应参数
                        Dictionary<String, String> dic = new Dictionary<string, string>();
                        dic.Add("ApiUserId", ConstUtility.ApiUserId);
                        dic.Add("Signature", Encrypt.MD5Encrypt(ConstUtility.ApiUserId + ConstUtility.Signature + DateTime.Now.ToString("yyyyMMdd")));
                        dic.Add("Phone", GetString(rowItemObj["Phone"]));
                        dic.Add("Name", GetString(rowItemObj["Name"]));
                        dic.Add("NickName", "");
                        dic.Add("Gender", GetString(rowItemObj["Male"]));
                        dic.Add("BirthDate", GetString(rowItemObj["Birthday"]));
                        dic.Add("Address", GetString(rowItemObj["Address"]));
                        dic.Add("Carrer", GetString(rowItemObj["Career"]));
                        dic.Add("Email", "");
                        dic.Add("IdCard", GetString(rowItemObj["SSN"]));

                        string input = DicToString(dic);
                        WebApiInvoker webPaiInvoker = new WebApiInvoker(ConstUtility.CRMURL);
                        String resultJson = webPaiInvoker.InvokePostRequest("Customer", "SyncCustomerInfo", dic).Result;
                        iLog.WriteLog(string.Format("更新租客表,调用CRM接口SyncCustomerInfo成功:输入参数：{0},输出参数：{1}    SIGNATURE:{2}", input, resultJson, ConstUtility.ApiUserId + ConstUtility.Signature + DateTime.Now.ToString("yyyyMMdd")), 0);

                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerResultCRM>(resultJson);
                        if (result != null && result.IsResult.ToLower() == "true")
                        {
                            updateSql.AppendLine(" UPDATE dbo.Renters SET CustomerCode = '" + result.Data + "' WHERE ID = '" + rowItemObj["ID"] + "'; ");
                        }
                        iLog.WriteLog("更新租客表,CustomerCode Exec SQL:" + updateSql.ToString(), 0);
                        if (updateSql.Length > 0)
                        {
                            sqlTransaction = sqlCon.BeginTransaction();
                            Util.ExecuteSqlNoReturn(sqlCon, sqlTransaction, updateSql.ToString());
                            sqlTransaction.Commit();
                            sqlTransaction = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                iLog.WriteLog(string.Format("更新租客表,调用CRM接口SyncCustomerInfo失败：" + e.Message), 0);
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                    sqlTransaction = null;
                }
                throw;
            }
            finally
            {
                if (sqlCon != null)
                    sqlCon.Close();
            }
        }

        /// <summary>
        /// CRM更新入住表CustomerCode
        /// </summary>
        /// <param name="connString"></param>
        public static void updateCheckInCustomerCodeCRM(string connString)
        {
            iLog.WriteLog("更新入住表,调用CRM接口SyncCustomerInfo开始...", 0);
            SqlConnection sqlCon = null;
            SqlTransaction sqlTransaction = null;
            String UnUpdateCustomerSQL = @"SELECT * FROM dbo.CheckIns WHERE Status=1 and CheckInType=1 AND ISNULL(CustomerCode,'')='' AND ISNULL(Phone,'')<>'' AND LEN(Phone)=11 ";

            try
            {
                //查询CustomerCode为空的CheckIns
                sqlCon = new SqlConnection(connString);
                sqlCon.Open();
                DataTable dtUnUpdateCustomer = Util.ExecuteSqlSelect(sqlCon, UnUpdateCustomerSQL);
                iLog.WriteLog("Customer count: " + dtUnUpdateCustomer.Rows.Count, 0);

                for (int j = 0; j < dtUnUpdateCustomer.Rows.Count; j++)
                {
                    StringBuilder updateSql = new StringBuilder();

                    DataRow rowItemObj = dtUnUpdateCustomer.Rows[j];
                    if (ConstUtility.GetCustomerCodeTimeOut != "0")
                    {
                        //添加对应参数
                        Dictionary<String, String> dic = new Dictionary<string, string>();
                        dic.Add("ApiUserId", ConstUtility.ApiUserId);
                        dic.Add("Signature", Encrypt.MD5Encrypt(ConstUtility.ApiUserId + ConstUtility.Signature + DateTime.Now.ToString("yyyyMMdd")));
                        dic.Add("Phone", GetString(rowItemObj["Phone"]));
                        dic.Add("Name", GetString(rowItemObj["Name"]));
                        dic.Add("NickName", "");
                        dic.Add("Gender", "");
                        dic.Add("BirthDate", GetString(rowItemObj["Birthday"]));
                        dic.Add("Address", GetString(rowItemObj["Address"]));
                        dic.Add("Carrer", "");
                        dic.Add("Email", "");
                        dic.Add("IdCard", GetString(rowItemObj["Card"]));

                        string input = DicToString(dic);
                        WebApiInvoker webPaiInvoker = new WebApiInvoker(ConstUtility.CRMURL);
                        String resultJson = webPaiInvoker.InvokePostRequest("Customer", "SyncCustomerInfo", dic).Result;
                        iLog.WriteLog(string.Format("更新入住表,调用CRM接口SyncCustomerInfo成功:输入参数：{0},输出参数：{1}    SIGNATURE:{2}", input, resultJson, ConstUtility.ApiUserId + ConstUtility.Signature + DateTime.Now.ToString("yyyyMMdd")), 0);

                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerResultCRM>(resultJson);
                        if (result != null && result.IsResult.ToLower() == "true")
                        {
                            updateSql.AppendLine(" UPDATE dbo.CheckIns SET CustomerCode = '" + result.Data + "' WHERE ID = '" + rowItemObj["ID"] + "'; ");
                        }
                        iLog.WriteLog("更新入住表,CustomerCode Exec SQL:" + updateSql.ToString(), 0);
                        if (updateSql.Length > 0)
                        {
                            sqlTransaction = sqlCon.BeginTransaction();
                            Util.ExecuteSqlNoReturn(sqlCon, sqlTransaction, updateSql.ToString());
                            sqlTransaction.Commit();
                            sqlTransaction = null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                iLog.WriteLog(string.Format("更新入住表,调用CRM接口SyncCustomerInfo失败：" + e.Message), 0);
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                    sqlTransaction = null;
                }
                throw;
            }
            finally
            {
                if (sqlCon != null)
                    sqlCon.Close();
            }
        }

        public static void UpdateCustomerCodeAll(string connString)
        {
            if (ConstUtility.CallFrom == 0)
            {
                updateCustomerCode(connString);
            }
            else
            {
                updateRenterCustomerCodeCRM(connString);
                updateCheckInCustomerCodeCRM(connString);
            }
        }

        public static string DicToString(Dictionary<String, String> dic)
        {
            String str = "";
            foreach (var item in dic)
            {
                str += "\"" + item.Key + "\":";
                str += "\"" + item.Value + "\",";
            }
            if (str.Length > 0)
            {
                str = str.Substring(0, str.Length - 1);
            }
            return "{" + str + "}";
        }

        public static String GetString(object value)
        {
            return value == null ? "" : value.ToString().Trim();
        }
    }

    public class CustomerResult
    {
        public String CustomerCode { get; set; }
        public String OtherSid { get; set; }
    }

    public class CustomerResultCRM
    {
        public String Code { get; set; }
        public String Msg { get; set; }
        public String IsResult { get; set; }
        public String Data { get; set; }
    }
}
