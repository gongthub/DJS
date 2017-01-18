using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroppowerWarning.Utils
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
            WebApiUrl = iConfigMgr.GetConfig("WebApiUrl");
            HyUserName = iConfigMgr.GetConfig("HyUserName");
            IDBRepository = iConfigMgr.GetConfig("IDBRepository");
            AmsType = iConfigMgr.GetConfig("AmsType");
            Ele = iConfigMgr.GetConfig("Ele");
            Clod = iConfigMgr.GetConfig("Clod");
            Hot = iConfigMgr.GetConfig("Hot");
        }

        #endregion
        //配置参数
        public static string WebApiUrl = "";
        public static string HyUserName = "";

        public static string IDBRepository = "";
        //ams类型 0：大V 1：小V
        public static string AmsType = "";
        public static string Ele = "";
        public static string Clod = "";
        public static string Hot = "";


    }
}
