using DayTrialService.Model;
using DayTrialService.Services;
using DayTrialService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Data.SqlClient;

namespace DayTrialService.Service
{
    public class DayTrialSer
    {

        public static DJS.SDK.ILog iLog = null;


        private static string SERVICECODE = "";

        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary> 

        #endregion

        #region 构造函数

        static DayTrialSer()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            SERVICECODE = ConstUtility.ServiceCode;
        }

        #endregion

        /// <summary>
        /// 租金贷预警
        /// </summary>
        public static void SendDayTrialEmail()
        {
            try
            {
                if (EmailUtility.ServiceIsStart(SERVICECODE))
                {
                    List<Model.AMSServiceSetEmail> emailLists = EmailUtility.GetServiceSetEmailStoresList(SERVICECODE);
                    if (emailLists != null && emailLists.Count > 0)
                    {
                        foreach (Model.AMSServiceSetEmail itemT in emailLists)
                        {
                            try
                            {
                                if (itemT.IsSum)
                                {
                                    itemT.StoreID = EmailUtility.GetStoreIds();
                                }
                                List<Model.Store> stores = GetStores(itemT.StoreID);
                                if (stores != null && stores.Count > 0)
                                {
                                    string storeName = null;
                                    StringBuilder sbTable = new StringBuilder();
                                    string str = "各位领导好！下面是今日" + ConstUtility.MailSubject + "。请审阅！</br>";
                                    string strt = "";
                                    sbTable.Append(str);
                                    sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                                    sbTable.Append("<tr><td>门店编号</td><td>门店名称</td><td>地点</td><td>日期</td><td>门店状态</td></tr>");
                                    foreach (var item in stores)
                                    {
                                        if (item.Status == 2)
                                        {
                                            strt = "运营";
                                        }

                                        if (item.Status == 4)
                                        {
                                            strt = "预销售";
                                        }

                                        if (item.Status == 5)
                                        {
                                            strt = "试运营";
                                        }

                                        sbTable.Append("<tr>");
                                        sbTable.Append("<td>" + item.StoreCode + "</td>");
                                        sbTable.Append("<td>" + item.Name + "</td>");
                                        sbTable.Append("<td>" + item.Address + "</td>");
                                        sbTable.Append("<td>" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "</td>");
                                        sbTable.Append("<td>" + strt + "</td>");
                                        sbTable.Append("</tr>");
                                        storeName = item.Name;
                                    }
                                    sbTable.Append("</table>");

                                    //List<string> users = GetUsers();
                                    //List<string> ccusers = GetCCUsers();

                                    List<string> users = EmailUtility.GetServiceSetEmailToById(SERVICECODE, itemT.ID);
                                    List<string> ccusers = EmailUtility.GetServiceSetEmailCcById(SERVICECODE, itemT.ID);

                                    string subjects = ConstUtility.MailSubject;
                                    int storeId = 0;
                                    if (int.TryParse(itemT.StoreID, out storeId))
                                    {
                                        subjects += "(" + storeName + ")";
                                    }

                                    EmailUtility.SendEmail(users, ccusers, null, subjects, sbTable.ToString());
                                    iLog.WriteLog("未日审邮件发送成功", 0);
                                }
                            }
                            catch (Exception ex)
                            {
                                iLog.WriteLog("未日审邮件发送失败" + ex.Message, 1);
                            }
                        }
                    }
                }
                else
                {
                    iLog.WriteLog("邮件配置服务不启用！", 0);
                }

            }
            catch
            {
                //iLog.WriteLog("Error " + e.Message, 1);
                throw;
            }
        }

        /// <summary>
        /// 获取未日审门店
        /// </summary>
        /// <param name="StoreID"></param>
        /// <returns></returns>
        private static List<Model.Store> GetStores()
        {
            try
            {
                List<Model.Store> models = new List<Store>();
                IDBRepository dbContext = new IDBRepository();
                string date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string sqls = @"select * from Stores
                                where ID not in(
                                select A.StoreID from StoreDailyChecks A
                                left join Stores B on A.StoreID=B.ID
                                where B.Status in(2,4,5) and A.checkdate ='{0}'
                                ) and Status in(2,4,5)";
                SqlParameter[] prars = new SqlParameter[] { 
                new SqlParameter("@date",date) 
                };

                models = dbContext.Database.SqlQuery<Model.Store>(sqls, prars).ToList();

                return models;
            }
            catch
            {
                //iLog.WriteLog("Error " + e.Message, 1);
                throw;
            }
        }
        /// <summary>
        /// 获取未日审门店
        /// </summary>
        /// <param name="StoreID"></param>
        /// <returns></returns>
        private static List<Model.Store> GetStores(string Ids)
        {
            try
            {
                List<Model.Store> models = new List<Store>();
                IDBRepository dbContext = new IDBRepository();
                string date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string sqls = @"select * from Stores
                                where ID not in(
                                select A.StoreID from StoreDailyChecks A
                                left join Stores B on A.StoreID=B.ID
                                where B.Status in(2,4,5) and A.checkdate =@date
                                ) and Status in(2,4,5) and ID in({0})";

                sqls = string.Format(sqls, Ids);
                SqlParameter[] prars = new SqlParameter[] { 
                new SqlParameter("@date",date)
                };

                models = dbContext.Database.SqlQuery<Model.Store>(sqls, prars).ToList();

                return models;
            }
            catch
            {
                //iLog.WriteLog("Error " + e.Message, 1);
                throw;
            }
        }

        /// <summary>
        /// 获取收件人
        /// </summary>
        /// <returns></returns>
        private static List<string> GetUsers()
        {
            List<string> user = new List<string>();
            string users = ConstUtility.MailUsers;
            if (!string.IsNullOrEmpty(users))
            {
                string[] userst = users.Split(';');
                if (userst != null && userst.Length > 0)
                {
                    foreach (string item in userst)
                    {
                        user.Add(item);
                    }
                }
            }
            return user;
        }
        /// <summary>
        /// 获取抄送人
        /// </summary>
        /// <returns></returns>
        private static List<string> GetCCUsers()
        {
            List<string> user = new List<string>();
            string users = ConstUtility.CCUsers;
            if (!string.IsNullOrEmpty(users))
            {
                string[] userst = users.Split(';');
                if (userst != null && userst.Length > 0)
                {
                    foreach (string item in userst)
                    {
                        user.Add(item);
                    }
                }
            }
            return user;
        }
    }
}
