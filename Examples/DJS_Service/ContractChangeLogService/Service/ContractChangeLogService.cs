using ContractChangeLogService.Model;
using ContractChangeLogService.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContractChangeLogService.Service
{
    public class ContractChangeLogService
    {
        public static DJS.SDK.ILog iLog = null;

        private static string ServiceCode = "";
        private static string SendMailSender = "";
        private static string SendMailPassword = "";
        private static string EmailSMTP = "";
        private static string EmailTitle = "";

        private IDBRepository dbContext = new IDBRepository();

        #region 构造函数
        static ContractChangeLogService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();

            ServiceCode = ConstUtility.ServiceCode;
            SendMailSender = ConstUtility.SendMailSender;
            SendMailPassword = ConstUtility.SendMailPassword;
            EmailSMTP = ConstUtility.EmailSMTP;
            EmailTitle = ConstUtility.EmailTitle;
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
                                string strTable = GetContractChangeLog(itemT.StoreID, out storeName);
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
        /// 未收费邮件内容
        /// </summary>
        private string GetContractChangeLog(string storeID, out string storeName)
        {
            try
            {
                storeName = null;
                List<ContractChangeLogView> cashList = dbContext.ContractChangeLogViews.Where(t => storeID.Contains(t.StoreID.ToString())).ToList();

                if (cashList != null && cashList.Count > 0)
                {
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<table><tr><td style='color: red;'>您好，以下是" + EmailTitle + "信息列表，请知晓！</td></tr></table></br>");
                    sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\">");
                    sbTable.Append(@"<tr><td>门店名称</td><td>房间</td><td>租客</td><td>合同号</td><td>合同开始时间</td><td>合同结束时间</td>
                                         <td>合同结束时间</td><td>合同状态</td><td>变更原因</td><td>变更人</td><td>变更时间</td><td>变更类型</td></tr>");
                    foreach (var item in cashList)
                    {
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>" + item.StoreName + "</td>");
                        sbTable.Append("<td>" + item.FullName + "</td>");
                        sbTable.Append("<td>" + item.Name + "</td>");
                        sbTable.Append("<td>" + item.ContractNo + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.StartDate).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.EndDate).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + Convert.ToDateTime(item.LastChangeEndDate).ToShortDateString() + "</td>");
                        sbTable.Append("<td>" + GetEnumDescription(typeof(EnumUtility.ContractStatus), item.Status) + "</td>");
                        sbTable.Append("<td>" + item.Reason + "</td>");
                        sbTable.Append("<td>" + item.UserName + "</td>");
                        sbTable.Append("<td>" + item.CreateDate + "</td>");
                        sbTable.Append("<td>" + item.DataStatus + "</td>");
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

        /// <summary>
        /// 共用方法--取枚举的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type type, object value)
        {
            string description = "";
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                FieldInfo field = fields[i];

                if (field.Name.Equals(Enum.GetName(type, value)))
                {
                    object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (objs == null || objs.Length == 0)
                    {
                        description = field.Name;
                    }
                    else
                    {
                        DescriptionAttribute da = (DescriptionAttribute)objs[0];
                        description = da.Description;
                    }
                }
            }
            return description;
        }
    }
}
