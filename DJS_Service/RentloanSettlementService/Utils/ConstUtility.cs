using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.Utils
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
            //iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr();
            iConfigMgr = Achieve.iConfigMgr;
            IDBEmailService = iConfigMgr.GetConfig("EFDbContext");
        }

        #endregion


        public static string IDBEmailService = "";

    }
}
