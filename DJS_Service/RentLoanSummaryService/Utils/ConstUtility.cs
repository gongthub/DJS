using System;

namespace RentLoanSummaryService.Utils
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

            WarningDate = iConfigMgr.GetConfig("WarningDate");

            EMAIL_SMTP = iConfigMgr.GetConfig("EmailSMTP");

            IDBEmailService = iConfigMgr.GetConfig("IDBRepositoryEmailService");

            NotApply = iConfigMgr.GetConfig("NotApply");

            NotAudit = iConfigMgr.GetConfig("NotAudit");

            AuditOK = iConfigMgr.GetConfig("AuditOK");

            AuditRefuse = iConfigMgr.GetConfig("AuditRefuse");

            OverdueNotRepay = iConfigMgr.GetConfig("OverdueNotRepay");

            //WaitIntoPiece = iConfigMgr.GetConfig("WaitIntoPiece");

            WaitSubmit = iConfigMgr.GetConfig("WaitSubmit");

            ServiceCode = iConfigMgr.GetConfig("ServiceCode");

            NormalRepayment = iConfigMgr.GetConfig("NormalRepayment");

            SystemName = iConfigMgr.GetConfig("SystemName");
        }

        #endregion
        //邮件发送邮箱
        public static string SEND_EMAIL_SENDER = "";
        public static string SEND_EMAIL_PASSWORD = "";
        //预警日期
        public static string WarningDate = "";

        //邮件发送服务
        public static string EMAIL_SMTP = "";

        public static string IDBEmailService = "";


        // 0）公共阀值：风控天数 20
        // 1）未申请超过5天，邮件预警提醒
        // 2）未审核超过15天，邮件预警提醒
        // 3）审核通过8天以后不放款，邮件预警提醒。
        // 4）审核不通过未处理，邮件预警提醒。
        // 5）逾期未还款45天，邮件预警提醒。

        // 6）待进件超15天，邮件预警提醒
        // 7）待提交超5天，邮件预警提醒
        public static string NotApply = "";
        public static string NotAudit = "";
        public static string AuditOK = "";
        public static string AuditRefuse = "";
        public static string OverdueNotRepay = "";
        //public static string WaitIntoPiece = "";
        public static string WaitSubmit = "";


        public static string NormalRepayment = "";      //5正常还款 小于等于0天
        public static string ServiceCode = "";

        public static string SystemName = "";
    }
}
