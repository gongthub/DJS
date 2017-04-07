using PMPRoomChangeService.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PMPRoomChangeService.Service
{
    public class RoomChangeService
    {
        public static DJS.SDK.ILog iLog = null;

        private static string SERVICECODE = "";

        #region 构造函数

        static RoomChangeService()
        {
            iLog = DJS.SDK.DataAccess.CreateILog();
            SERVICECODE = ConstUtility.ServiceCode;
        }

        #endregion

        public void SendEmail(string jobName)
        {
            try
            {
                try
                {
                    Achieve ach = new Achieve();
                    string strlastTimes = ConstUtility.LastChangeTime;
                    DateTime dtLastTime = Convert.ToDateTime("2000-01-01 00:00:00");
                    if (!DateTime.TryParse(strlastTimes, out dtLastTime))
                    {
                        dtLastTime = Convert.ToDateTime("2000-01-01 00:00:00");
                    }
                    List<Model.ProjectUpdateLog> lsProjectUpLogs = GetUpLogs(dtLastTime);
                    if (lsProjectUpLogs != null && lsProjectUpLogs.Count > 0)
                    {
                        string strHtmls = GetUplogsHtmlStr(lsProjectUpLogs);
                        if (EmailService.ServiceIsStart(SERVICECODE))
                        {
                            List<Model.AMSServiceSetEmail> emailLists = EmailService.GetServiceSetEmailStoresList(SERVICECODE);
                            if (emailLists != null && emailLists.Count > 0)
                            {
                                foreach (Model.AMSServiceSetEmail itemT in emailLists)
                                {
                                    List<string> users = EmailService.GetServiceSetEmailToById(SERVICECODE, itemT.ID);
                                    List<string> ccusers = EmailService.GetServiceSetEmailCcById(SERVICECODE, itemT.ID);

                                    string subjects = ConstUtility.EmailTitle;
                                    EmailService.SendEmail(users, ccusers, null, subjects, strHtmls);
                                    iLog.WriteLog("PMP房间数量改变发送成功", 0);
                                }
                            }

                            DateTime? ndtLastTime = lsProjectUpLogs.OrderByDescending(m => m.CreateUserDate).Select(m => m.CreateUserDate).FirstOrDefault();
                            if (ndtLastTime != null)
                                strlastTimes = ((DateTime)ndtLastTime).ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                strlastTimes = "";
                        }
                        ach.UpdateConfig(jobName, "LastChangeTime", strlastTimes);
                    }
                }
                catch (Exception ex)
                {
                    iLog.WriteLog("PMP房间数量改变发送失败" + ex.Message, 1);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据最后发送时间获取更新日志
        /// </summary>
        /// <param name="lastTime"></param>
        /// <returns></returns>
        private List<Model.ProjectUpdateLog> GetUpLogs(DateTime lastTime)
        {
            List<Model.ProjectUpdateLog> updateLogs = new List<Model.ProjectUpdateLog>();
            IDBRepository dbContext = new IDBRepository();

            string strSqls = @"select A.*,B.Name as ProjectName,C.ItemName,D.UserName from ProjectUpdateLogs A
                                left join Projects B ON A.ProjectID=B.ID
                                Left JOIN Items C ON B.ItemID=C.Id
                                left JOIN UserProfiles D ON A.CreateUserID=D.UserId
                                where A.CreateUserDate >@lastTimes";

            updateLogs = dbContext.Database.SqlQuery<Model.ProjectUpdateLog>(strSqls, new SqlParameter("@lastTimes", lastTime)).ToList();
            return updateLogs;
        }

        /// <summary>
        /// 获取发送邮件html字符串
        /// </summary>
        /// <param name="lsProjectUpLogs"></param>
        /// <returns></returns>
        private string GetUplogsHtmlStr(List<Model.ProjectUpdateLog> lsProjectUpLogs)
        {
            string strHtmls = string.Empty;

            StringBuilder sbTable = new StringBuilder();
            string str = "<div style='font-size:16px;'>";
            str += "各位领导好！下面是" + ConstUtility.EmailTitle + "。请知晓！</br></br>";
            sbTable.Append(str);
            sbTable.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\" style='text-align:center;font-size:16px;'>");
            sbTable.Append("<tr><td style='white-space:nowrap;'>项目名称</td><td style='white-space:nowrap;'>工期名称</td><td style='white-space:nowrap;'>修改人</td><td style='white-space:nowrap;'>修改时间</td><td style='white-space:nowrap;'>原可售房间数</td><td style='white-space:nowrap;'>原可售床位数</td><td style='white-space:nowrap;'>原不可售房间数</td><td style='white-space:nowrap;'>原不可售床位数</td><td style='white-space:nowrap;'>现可售房间数</td><td style='white-space:nowrap;'>现可售床位数</td><td style='white-space:nowrap;'>现不可售房间数</td><td style='white-space:nowrap;'>现不可售床位数</td><td style='white-space:nowrap;'>变更原因</td></tr>");
            for (int i = 0; i < lsProjectUpLogs.Count; i++)
            {
                if (i % 2 == 0)
                {
                    sbTable.Append("<tr style='background-color: #DBDBDB'>");
                }
                else
                {
                    sbTable.Append("<tr>");
                }
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].ItemName + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].ProjectName + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].UserName + "</td>");
                if (lsProjectUpLogs[i].CreateUserDate != null)
                {
                    sbTable.Append("<td style='white-space:nowrap;'>" + ((DateTime)lsProjectUpLogs[i].CreateUserDate).ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                }
                else
                {
                    sbTable.Append("<td></td>");
                }
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].OldRoomNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].OldBedNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].OldNotSellRoomNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].OldNotSellBedNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].RoomNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].BedNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].NotSellRoomNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].NotSellBedNumber + "</td>");
                sbTable.Append("<td style='white-space:nowrap;'>" + lsProjectUpLogs[i].ModifyReason + "</td>");
                sbTable.Append("</tr>");
            }
            sbTable.Append("</table>");
            strHtmls = sbTable.ToString();
            strHtmls += "</br>本邮件发送自微领地PMP平台 （操作者：PMP系统）";
            strHtmls += "</div>";
            return strHtmls;
        }
    }
}
