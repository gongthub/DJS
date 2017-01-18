using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Utils
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

            SavesFilePath = iConfigMgr.GetConfig("SavesFilePath");
            ZipFilePath = iConfigMgr.GetConfig("ZipFilePath");

            ConnType = iConfigMgr.GetConfig("ConnType");
            ConnStr = iConfigMgr.GetConfig("ConnStr");

            SendEmail = iConfigMgr.GetConfig("SendEmail");
            SendPwd = iConfigMgr.GetConfig("SendPwd");

            Temp_DaylReportSum = iConfigMgr.GetConfig("Temp_DaylReportSum");

            ALLdayrepor = iConfigMgr.GetConfig("ALLdayrepor");
            StoreFileName = iConfigMgr.GetConfig("StoreFileName");
            DayRepoertFileName = iConfigMgr.GetConfig("DayRepoertFileName");
            UserStoreName = iConfigMgr.GetConfig("UserStoreName");
            Suffix = iConfigMgr.GetConfig("Suffix");
            EmailTitle = iConfigMgr.GetConfig("EmailTitle");
            EmailDesc = iConfigMgr.GetConfig("EmailDesc");
            EmailReportName = iConfigMgr.GetConfig("EmailReportName");

            ServiceCode = iConfigMgr.GetConfig("ServiceCode");
        }

        #endregion
        public static string SavesFilePath ="";
        public static string ZipFilePath = "";

        public static string ConnType = "";
        public static string ConnStr = "";

        public static string SendEmail = "";
        public static string SendPwd = "";

        public static string Temp_DaylReportSum = "";

        public static string ALLdayrepor = "";
        public static string StoreFileName = "";
        public static string DayRepoertFileName = "";
        public static string UserStoreName = "";
        public static string Suffix = "";
        public static string EmailTitle = "";
        public static string EmailDesc = "";
        public static string EmailReportName = "";
        public static string ServiceCode = ""; 
    }
}
