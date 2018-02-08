using DayTrialService;
using System;

namespace DayTrialService.Utils
{
    public class ConstUtility
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static DJS.SDK.IConfigMgr iConfigMgr = Achieve.iConfigMgr;

        #endregion

        #region 构造函数

        public ConstUtility()
        {
            //iLog = DJS.SDK.DataAccess.CreateILog();
            iConfigMgr = Achieve.iConfigMgr;
            //邮件发送邮箱
            SEND_EMAIL_SENDER = iConfigMgr.GetConfig("SendMailSender");
            SEND_EMAIL_PASSWORD = iConfigMgr.GetConfig("SendMailPassword");
             
            EMAIL_SMTP = iConfigMgr.GetConfig("EmailSMTP");

            IDBRepository = iConfigMgr.GetConfig("IDBRepository");
             
            MailUsers = iConfigMgr.GetConfig("MailUsers");
            CCUsers = iConfigMgr.GetConfig("CCUsers");
            MailSubject = iConfigMgr.GetConfig("MailSubject");
            ServiceCode = iConfigMgr.GetConfig("ServiceCode");
        }

        #endregion
        //邮件发送邮箱
        public static string SEND_EMAIL_SENDER = "";
        public static string SEND_EMAIL_PASSWORD = ""; 

        //邮件发送服务
        public static string EMAIL_SMTP = "";

        public static string IDBRepository = "";

        public static string MailUsers = "";

        public static string CCUsers = "";

        public static string MailSubject = "";
        public static string ServiceCode = "";

    }
}
