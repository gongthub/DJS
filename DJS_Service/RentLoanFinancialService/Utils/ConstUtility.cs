using DJS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Utils
{
    public class ConstUtility
    {
        private static IConfigMgr iConfigMgr = Achieve.iConfigMgr;
        public ConstUtility()
        {
            IDBRepositoy = iConfigMgr.GetConfig("IDBRepositoy");
            crmURL = iConfigMgr.GetConfig("crmURL");
            APIUSERID = iConfigMgr.GetConfig("APIUSERID");
            SIGNATURE = iConfigMgr.GetConfig("SIGNATURE");
            PullStartDateTime = iConfigMgr.GetConfig("PullStartDateTime");
        }
        public static string IDBRepositoy = "";
        public static string crmURL = "";
        public static string APIUSERID = "";
        public static string SIGNATURE = "";
        public static string PullStartDateTime = "";
    }
}
