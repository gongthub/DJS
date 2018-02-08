using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncCustomerCodeService.Utils
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

            OfficialWebServiceURL = iConfigMgr.GetConfig("OfficialWebServiceURL");
            IDBRepositorySRC = iConfigMgr.GetConfig("IDBRepository");

            CRMURL = iConfigMgr.GetConfig("CRMURL");
            ApiUserId = iConfigMgr.GetConfig("ApiUserId");
            Signature = iConfigMgr.GetConfig("Signature");
            GetCustomerCodeTimeOut = iConfigMgr.GetConfig("GetCustomerCodeTimeOut");
            CallFrom = Convert.ToInt32(iConfigMgr.GetConfig("CallFrom"));
        }

        #endregion
        public static string OfficialWebServiceURL = "";
        public static string IDBRepositorySRC = "";

        public static string CRMURL = "";
        public static string ApiUserId = "";
        public static string Signature = "";
        public static string GetCustomerCodeTimeOut = "";

        public static int CallFrom = 0;

    }
}
