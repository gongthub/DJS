using ShopChargeWarnService.Model;
using ShopChargeWarnService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopChargeWarnService.Service
{
    public class ShopChargeWarnService
    {

        public static DJS.SDK.ILog iLog = null;

        private static string ServiceCode = "";
        private static string SendMailSender = "";
        private static string SendMailPassword = "";
        private static string EmailSMTP = "";
        private static string SystemFlag = "";
        private static string EmailTitle = "";

        private IDBRepository dbContext = new IDBRepository();

        #region 构造函数
        static ShopChargeWarnService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();

            ServiceCode = ConstUtility.ServiceCode;
            SendMailSender = ConstUtility.SendMailSender;
            SendMailPassword = ConstUtility.SendMailPassword;
            EmailSMTP = ConstUtility.EmailSMTP;
            SystemFlag = ConstUtility.SystemFlag;
            EmailTitle = ConstUtility.EmailTitle;
        }
        #endregion

        /// <summary>
        /// 发送底商收费提醒邮件
        /// </summary>
        public void SendShopCharge()
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
                                string strTable = GetShopCharge(itemT.StoreID, out storeName);
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
        /// 底商收费邮件内容
        /// </summary>
        private string GetShopCharge(string storeID, out string storeName)
        {
            try
            {
                storeName = null;
                List<ShopChargeWarnView> cashList = dbContext.ShopChargeWarnViews.Where(t => storeID.Contains(t.StoreID.ToString())).ToList();

                if (cashList != null && cashList.Count > 0)
                {
                    cashList = cashList.OrderBy(t => t.StoreID).ThenBy(t => t.DayCount).ToList();
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<table><tr><td style='color: red;'>您好，以下是截止今日" + SystemFlag + "底商收费提醒列表，特此提醒，请知晓！</td></tr></table></br>");
                    sbTable.Append("<table style='width:1800px;' cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                    sbTable.Append(@"<tr><td style='width:200px;'>门店名称</td>
                                         <td style='width:200px;'>房间号</td>
                                         <td style='width:200px;'>合同号</td>
                                         <td style='width:200px;'>租户</td>
                                         <td style='width:100px;'>租户电话</td>
                                         <td style='width:100px;'>房价</td>
                                         <td style='width:100px;'>合同开始日期</td>
                                         <td style='width:100px;'>合同结束日期</td>
                                         <td style='width:100px;'>合同类型</td>
                                         <td style='width:100px;'>是否扣点底商</td>
                                         <td style='width:100px;'>类型</td>
                                         <td style='width:150px;'>上次激费覆盖日期</td>
                                         <td style='width:150px;'>距离应激费日期天数</td></tr>");
                    foreach (var item in cashList)
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>" + item.StoreName + "</td>");
                        sbTable.Append("<td>" + item.FullName + "</td>");
                        sbTable.Append("<td>" + item.ContractNo + "</td>");
                        sbTable.Append("<td>" + item.RenterName + "</td>");
                        sbTable.Append("<td>" + item.Phone + "</td>");
                        sbTable.Append("<td>" + item.CharterMoney + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.ContractStartDate).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.ContractEndDate).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + item.ContractType + "</td>");
                        sbTable.Append("<td>" + item.IsPointShop + "</td>");
                        sbTable.Append("<td>" + item.SpecialRoomType + "</td>");
                        sbTable.Append("<td>" + (Convert.ToDateTime(item.PCStartDate).ToShortDateString() + "~" + Convert.ToDateTime(item.PCEndDate).ToShortDateString()) + "</td>");
                        if (item.DayCount <= 0)
                            sbTable.Append("<td style='color:red;'>" + item.DayCount + "</td>");
                        else
                            sbTable.Append("<td>" + item.DayCount + "</td>");

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
