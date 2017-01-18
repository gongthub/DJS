using ReserveOverdueService.Model;
using ReserveOverdueService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserveOverdueService.Service
{
    public class ReserveOverdueService
    {
        public static DJS.SDK.ILog iLog = null;
        private static string serviceCode = "";
        private static string emailTitle = "";
        private IDBRepository dbContext = new IDBRepository();
        private static string SERVICECODE = "";

        #region 构造函数
        static ReserveOverdueService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            serviceCode = ConstUtility.ServiceCode;
            emailTitle = ConstUtility.EmailTitle;
            SERVICECODE = ConstUtility.ServiceCode;
        }

        #endregion

        /// <summary>
        /// 发送预订过期邮件
        /// </summary>
        /// <param name="CategoryID"></param>
        public void SendOverdueEmail()
        {
            try
            {
                if (EmailService.ServiceIsStart(SERVICECODE))
                {
                    List<Model.AMSServiceSetEmail> emailLists = EmailService.GetServiceSetEmailStoresList(SERVICECODE);
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
                                string strTable = SendReserveEmail(itemT.StoreID, out storeName);
                                string titleTemp = null;
                                int storeId = 0;
                                if (int.TryParse(itemT.StoreID, out storeId))
                                {
                                    titleTemp = "(" + storeName + ")";
                                }

                                if (!string.IsNullOrEmpty(strTable))
                                {
                                    List<string> users = EmailService.GetServiceSetEmailToById(SERVICECODE, itemT.ID);
                                    List<string> ccusers = EmailService.GetServiceSetEmailCcById(SERVICECODE, itemT.ID);

                                    EmailService.SendEmail(users, ccusers, null, emailTitle + titleTemp, strTable);
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
        /// 预订过期邮件
        /// </summary>
        public string SendReserveEmail(string storeID, out string storeName)
        {
            try
            {
                storeName = null;
                List<ReserveOverdue> overdueList = dbContext.ReserveOverdues.Where(t => storeID.Contains(t.StoreID.ToString())).OrderBy(t => t.StoreID).ThenBy(t => t.CheckInDate).ToList();

                if (overdueList != null && overdueList.Count > 0)
                {
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<table><tr><td style='color: red;'>您好，以下是截止今日预订超期的清单，请知晓！</td></tr></table></br>");
                    sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                    sbTable.Append("<tr><td>门店</td><td>房间</td><td>预订人</td><td>预订人手机号</td><td>预订号</td><td>预订时间</td><td>预计入住时间</td><td>超期天数</td><td>入住人</td></tr>");
                    foreach (var item in overdueList)
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>" + item.StoreName + "</td>");
                        sbTable.Append("<td>" + item.FullName + "</td>");
                        sbTable.Append("<td>" + item.YuDingName + "</td>");
                        sbTable.Append("<td>" + item.YuDingPhone + "</td>");
                        sbTable.Append("<td>" + item.ReservationNo + "</td>");
                        sbTable.Append("<td>" + item.CreateDate.ToString("yyyy/MM/dd HH:mm") + "</td>");
                        sbTable.Append("<td>" + item.CheckInDate.ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + item.OverdueDay + "</td>");
                        sbTable.Append("<td>" + item.RenterName + "</td>");
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
