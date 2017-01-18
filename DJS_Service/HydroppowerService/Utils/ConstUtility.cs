using HydroppowerService;
using System;

namespace HydroppowerService.Utils
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
        }

        #endregion
        //邮件发送邮箱
        public static string WebApiUrl = "";
        public static string HyUserName = ""; 
         
        public static string IDBRepository = "";
         

    }
}
