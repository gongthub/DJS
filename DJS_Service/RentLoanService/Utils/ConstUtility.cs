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

            WarningDate = iConfigMgr.GetConfig("WarningDate");

            EMAIL_SMTP = iConfigMgr.GetConfig("EmailSMTP");

            IDBEmailService = iConfigMgr.GetConfig("IDBRepositoryEmailService");

            NotApply = iConfigMgr.GetConfig("NotApply");

            NotAudit = iConfigMgr.GetConfig("NotAudit");

            AuditOK = iConfigMgr.GetConfig("AuditOK");

            AuditRefuse = iConfigMgr.GetConfig("AuditRefuse");

            OverdueNotRepay = iConfigMgr.GetConfig("OverdueNotRepay");

            WaitIntoPiece = iConfigMgr.GetConfig("WaitIntoPiece");

            WaitSubmit = iConfigMgr.GetConfig("WaitSubmit");

            ServiceCode = iConfigMgr.GetConfig("ServiceCode");

            NormalRepayment = iConfigMgr.GetConfig("NormalRepayment");
        }

        #endregion
        //邮件发送邮箱
        public static string SEND_EMAIL_SENDER = "";
        public static string SEND_EMAIL_PASSWORD = "";

        public static string WarningDate = "";          //预警日期 20天

        //邮件发送服务
        public static string EMAIL_SMTP = "";
        public static string IDBEmailService = "";

        public static string NotApply = "";             //100未申请租金贷 5天
        public static string NotAudit = "";             //1待审核，未审核超过7天
        public static string AuditOK = "";              //2审核通过8天以后不放款
        public static string AuditRefuse = "";          //3审核未通过
        public static string OverdueNotRepay = "";      //9逾期未还款 45天
        public static string WaitIntoPiece = "";        //10待进件 15天
        public static string WaitSubmit = "";           //0待提交 5天

        public static string NormalRepayment = "";      //5正常还款 小于等于0天

        public static string ServiceCode = "";
    }
}
