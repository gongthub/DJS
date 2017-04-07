using System;

namespace RentLoanService.Utils
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
            IDBEmailService = iConfigMgr.GetConfig("IDBRepositoryEmailService");
            ServiceCode = iConfigMgr.GetConfig("ServiceCode");

            WarningDate = iConfigMgr.GetConfig("WarningDate");
            MinWarningDate = iConfigMgr.GetConfig("MinWarningDate");
        }

        #endregion
        //邮件发送邮箱
        public static string SEND_EMAIL_SENDER = "";
        public static string SEND_EMAIL_PASSWORD = "";
        //邮件发送服务
        public static string EMAIL_SMTP = "";
        public static string IDBEmailService = "";
        public static string ServiceCode = "";

        public static string WarningDate = "";          //预警日期 45天
        public static string MinWarningDate = "";          //预警日期 5天
    }
}
