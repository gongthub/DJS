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

        public static void updateCustomerCode(string connString)
        {
            iLog.WriteLog("doUpdateCustomerCode Begin",0);
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
               iLog.WriteLog("Customer count: " + dtUnUpdateCustomer.Rows.Count,0);
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
                            if(!String.IsNullOrWhiteSpace(GetString(rowItemObj["Male"]))){
                                sex = (GetString(rowItemObj["Male"]) == "True")?"1":"0";
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

                   iLog.WriteLog("input:   "+unUpdateCustomerJson.ToString(),0);
                    dynamic customeData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(resultJson);
                   iLog.WriteLog("result:  "+resultJson,0);

                    //错误信息不为空则记录错误日志
                    if (customeData.Error.ToString() != "[]")
                    {
                        String strError = "";
                        for (int i = 0; i < customeData.Error.Count; i++)
                        {
                            strError += "{OtherSid：" + customeData.Error[i].OtherSid + ";Type：" + customeData.Error[i].Type + ";Description：" + customeData.Error[i].Description + "};";
                        }
                       iLog.WriteLog(string.Format("调用官网接口SyncCustomerInfo失败：{0}", strError),0);
                    }

                    List<CustomerResult> resultCustomers = null;
                    try
                    {
                        if (customeData.Data != null && customeData.Data.ToString() != "[]")
                            resultCustomers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerResult>>(customeData.Data.ToString());
                    }
                    catch (Exception ex)
                    {
                       iLog.WriteLog("doUpdateCustomerCode Error " + ex.Message,1);
                        continue;
                    }

                    if (resultCustomers != null && resultCustomers.Count > 0)
                    {
                        foreach (CustomerResult item in resultCustomers)
                        {
                            updateSql.AppendLine(" UPDATE dbo.Renters SET CustomerCode = '" + item.CustomerCode + "' WHERE ID = '" + item.OtherSid + "'; ");
                        }
                    }
                   iLog.WriteLog("doUpdateCustomerCode Exec SQL:" + updateSql.ToString(),0);
                    if (updateSql.Length > 0)
                    {
                        sqlTransaction = sqlCon.BeginTransaction();
                        Util.ExecuteSqlNoReturn(sqlCon, sqlTransaction, updateSql.ToString());
                        sqlTransaction.Commit();
                        sqlTransaction = null;
                    }
                }
            }
            catch (Exception ex)
            {
               iLog.WriteLog("doUpdateCustomerCode Error " + ex.Message,1);
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                    sqlTransaction = null;
                }
            }
            finally
            {
                if (sqlCon != null)
                    sqlCon.Close();
            }
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
}
