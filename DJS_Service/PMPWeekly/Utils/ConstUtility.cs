using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPWeekly.Utils
{
   public  class ConstUtility
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
            EmailTitle = iConfigMgr.GetConfig("EmailTitle");

            IDBAMSRepository = iConfigMgr.GetConfig("IDBAMSRepository");
        }

        #endregion

        public static string ServiceCode = "";
        public static string SendMailSender = "";
        public static string SendMailPassword = "";
        public static string EmailSMTP = "";
        public static string IDBRepository = "";

        public static string EmailTitle = "";


        public static string IDBAMSRepository = "";
    }
}
