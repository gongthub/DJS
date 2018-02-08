using DayTrialAfterCashService.Model;
using DayTrialAfterCashService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayTrialAfterCashService.Service
{
    public class DayTrialAfterCashService
    {
        public static DJS.SDK.ILog iLog = null;

        private static string ServiceCode = "";
        private static string SendMailSender = "";
        private static string SendMailPassword = "";
        private static string EmailSMTP = "";
        private static string SystemFlag = "";
        private static string EmailTitle = "";
        private static int Cash = 0;

        private IDBRepository dbContext = new IDBRepository();

        #region 构造函数
        static DayTrialAfterCashService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();

            ServiceCode = ConstUtility.ServiceCode;
            SendMailSender = ConstUtility.SendMailSender;
            SendMailPassword = ConstUtility.SendMailPassword;
            EmailSMTP = ConstUtility.EmailSMTP;
            SystemFlag = ConstUtility.SystemFlag;
            EmailTitle = ConstUtility.EmailTitle;
            Cash = ConstUtility.Cash;
        }
        #endregion

        /// <summary>
        /// 发送门店日审后现金提醒邮件
        /// </summary>
        public void SendDayTrialAfterCash()
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
                                string strTable = GetDayTrialAfterCash(itemT.StoreID, out storeName);
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

                                    EmailService.SendEmail(users, ccusers, null, SystemFlag + EmailTitle + titleTemp, strTable);
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
        /// 未收费邮件内容
        /// </summary>
        private string GetDayTrialAfterCash(string storeID, out string storeName)
        {
            try
            {
                storeName = null;
                List<DayTrialAfterCashView> cashList = dbContext.DayTrialAfterCashViews.Where(t => storeID.Contains(t.StoreID.ToString())).ToList();

                if (cashList != null && cashList.Count > 0)
                {
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<table><tr><td style='color: red;'>您好，以下是截止昨日日审结束后结余现金大于" + Cash + "元的" + SystemFlag + "门店列表，特此提醒，请知晓！</td></tr></table></br>");
                    sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                    sbTable.Append("<tr><td>门店编号</td><td>门店名称</td><td>门店地址</td><td>结余日期</td><td>结余现金</td><td>上次存银行日期</td></tr>");
                    foreach (var item in cashList)
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>" + item.StoreCode + "</td>");
                        sbTable.Append("<td>" + item.StoreName + "</td>");
                        sbTable.Append("<td>" + item.StoreAddress + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.Date).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + item.RemainingCash + "</td>");
                        if (item.UpSaveCashDate == null)
                        {
                            sbTable.Append("<td></td>");
                        }
                        else
                        {
                            sbTable.Append("<td>" + Convert.ToDateTime(item.UpSaveCashDate).ToShortDateString() + "</td>");
                        }
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
