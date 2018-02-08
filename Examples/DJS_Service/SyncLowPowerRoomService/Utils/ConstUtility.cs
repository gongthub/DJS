using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLowPowerRoomService.Utils
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
            WebApiUrl = iConfigMgr.GetConfig("WebApiUrl");
            PartnerId = iConfigMgr.GetConfig("PartnerId");
            Electric = iConfigMgr.GetConfig("Electric");
            SystemType = int.TryParse(iConfigMgr.GetConfig("SystemType"), out SystemType) ? SystemType : 0;
            ElectricWarn = int.TryParse(iConfigMgr.GetConfig("ElectricWarn"), out ElectricWarn) ? ElectricWarn : 0;
            IDBRepository = iConfigMgr.GetConfig("IDBRepository");
        }

        #endregion

        public static string WebApiUrl = "";
        public static string PartnerId = "";
        public static string Electric = "";
        public static int SystemType = 0;
        public static int ElectricWarn = 0;

        public static string IDBRepository = "";
    }
}
