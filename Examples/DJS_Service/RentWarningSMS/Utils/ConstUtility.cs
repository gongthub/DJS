using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentWarningSMS.Utils
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

            IDBRepository = iConfigMgr.GetConfig("IDBRepository");
            WarningDate = iConfigMgr.GetConfig("WarningDate");
            WebApiUrl = iConfigMgr.GetConfig("WebApiUrl");
            AmsType = iConfigMgr.GetConfig("AmsType");
            IsAddNotice = iConfigMgr.GetConfig("IsAddNotice");
            IsSendSMS = iConfigMgr.GetConfig("IsSendSMS"); 
        }


        #endregion

        public static string IDBRepository = "";
        public static string WarningDate = "";
        //配置参数
        public static string WebApiUrl = "";
        //ams类型 0：大V 1：小V
        public static string AmsType = "";

        //是否发送站内信 0：否 1：是
        public static string IsAddNotice = ""; 

        //是否发送短信 0：否 1：是
        public static string IsSendSMS = ""; 
    }
}
