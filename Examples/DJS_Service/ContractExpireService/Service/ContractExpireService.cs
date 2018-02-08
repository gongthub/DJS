using ContractExpireService.Model;
using ContractExpireService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractExpireService.Service
{
    public class ContractExpireService
    {
        public static DJS.SDK.ILog iLog = null;

        private static string ServiceCode = "";
        private static string SendMailSender = "";
        private static string SendMailPassword = "";
        private static string EmailSMTP = "";
        private static int WarningDate = 0;
        private static string EmailTitle = "";

        private IDBRepository dbContext = new IDBRepository();

        #region 构造函数
        static ContractExpireService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();

            ServiceCode = ConstUtility.ServiceCode;
            SendMailSender = ConstUtility.SendMailSender;
            SendMailPassword = ConstUtility.SendMailPassword;
            EmailSMTP = ConstUtility.EmailSMTP;
            WarningDate = ConstUtility.WarningDate;
            EmailTitle = ConstUtility.EmailTitle;
        }
        #endregion

        /// <summary>
        /// 发送合同到期提醒邮件
        /// </summary>
        public void SendStoreExpire()
        {
            try
            {
                if (EmailService.ServiceIsStart(ServiceCode))
                {
                    List<Model.AMSServiceSetEmail> emailLists = EmailService.GetServiceSetEmailStoresList(ServiceCode);
                    if (emailLists != null && emailLists.Count > 0)
                    {
                        foreach (Model.AMSServiceSetEmail itemT in emailLists)
                        {
                            try
                            {
                                if (itemT.IsSum)
                                {
                                    itemT.StoreID = EmailService.GetStoreIds();
                                }
                                string storeName = null;
                                string strTable = GetStoreExpire(itemT.StoreID, out storeName);
                                string titleTemp = null;
                                int storeId = 0;
                                if (int.TryParse(itemT.StoreID, out storeId))
                                {
                                    titleTemp = "(" + storeName + ")";
                                }

                                if (!string.IsNullOrEmpty(strTable))
                                {
                                    List<string> users = EmailService.GetServiceSetEmailToById(ServiceCode, itemT.ID);
                                    List<string> ccusers = EmailService.GetServiceSetEmailCcById(ServiceCode, itemT.ID);

                                    EmailService.SendEmail(users, ccusers, null, EmailTitle + titleTemp, strTable);
                                }
                            }
                            catch (Exception ex)
                            {
                                iLog.WriteLog(ex.Message, 1);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 合同过期邮件内容
        /// </summary>
        private string GetStoreExpire(string storeID, out string storeName)
        {
            try
            {
                storeName = null;
                List<ContractExpireView> overdueList = dbContext.ContractExpireViews.Where(t => storeID.Contains(t.StoreID.ToString())).OrderBy(t => t.StoreID).ThenBy(t => t.ExpireDay).ToList();

                if (overdueList != null && overdueList.Count > 0)
                {
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<table><tr><td style='color: red;'>您好，以下是截止今日未来" + WarningDate + "天" + EmailTitle + "清单，请知晓！</td></tr></table></br>");
                    sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                    sbTable.Append("<tr><td>门店</td><td>房间</td><td>租客</td><td>租客手机号</td><td>合同号</td><td>合同开始日期</td><td>合同结束日期</td><td>合同类型</td><td>到期天数</td></tr>");
                    foreach (var item in overdueList)
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>" + item.StoreName + "</td>");
                        sbTable.Append("<td>" + item.FullName + "</td>");
                        sbTable.Append("<td>" + item.RenterName + "</td>");
                        sbTable.Append("<td>" + item.Phone + "</td>");
                        sbTable.Append("<td>" + item.ContractNo + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.StartDate).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.EndDate).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + item.ContractType + "</td>");
                        sbTable.Append("<td>" + item.ExpireDay + "</td>");
                        sbTable.Append("</tr>");
                        storeName = item.StoreName;
                    }
                    sbTable.Append("</table>");
                    return sbTable.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
