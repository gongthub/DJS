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
        }

        #endregion
        public static string OfficialWebServiceURL = "";

        public static string IDBRepositorySRC = "";

    }
}
