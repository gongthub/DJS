using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserveOverdueService.Utils
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

            ServiceCode = iConfigMgr.GetConfig("ServiceCode");
            EmailTitle = iConfigMgr.GetConfig("EmailTitle");

            IDBRepository = iConfigMgr.GetConfig("IDBRepository");
        }

        #endregion
        //邮件发送邮箱
        public static string SEND_EMAIL_SENDER = "";
        public static string SEND_EMAIL_PASSWORD = "";

        //邮件发送服务
        public static string EMAIL_SMTP = "";

        public static string ServiceCode = "";
        public static string EmailTitle = "";

        public static string IDBRepository = "";

    }
}
