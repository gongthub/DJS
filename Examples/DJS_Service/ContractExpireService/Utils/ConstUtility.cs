using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractExpireService.Utils
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
            iConfigMgr = Achieve.iConfigMgr;

            ServiceCode = iConfigMgr.GetConfig("ServiceCode");
            SendMailSender = iConfigMgr.GetConfig("SendMailSender");
            SendMailPassword = iConfigMgr.GetConfig("SendMailPassword");
            EmailSMTP = iConfigMgr.GetConfig("EmailSMTP");
            IDBRepository = iConfigMgr.GetConfig("IDBRepository");

            WarningDate = int.TryParse(iConfigMgr.GetConfig("WarningDate"), out WarningDate) ? WarningDate : 0;
            EmailTitle = iConfigMgr.GetConfig("EmailTitle");
        }

        #endregion

        public static string ServiceCode = "";
        public static string SendMailSender = "";
        public static string SendMailPassword = "";
        public static string EmailSMTP = "";
        public static string IDBRepository = "";

        public static int WarningDate = 0;
        public static string EmailTitle = "";

    }
}
