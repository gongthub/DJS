using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToDayReportSumExcel.Utils
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
            Suffix = iConfigMgr.GetConfig("Suffix");

            EmailReportName = iConfigMgr.GetConfig("EmailReportName");

            Temp_DayReprotSum = iConfigMgr.GetConfig("Temp_DayReprotSum");
            EmailTitleDayReprotSum = iConfigMgr.GetConfig("EmailTitleDayReprotSum");
            EmailDescDayReprotSum = iConfigMgr.GetConfig("EmailDescDayReprotSum");
            ServiceCode = iConfigMgr.GetConfig("ServiceCode");
        }

        #endregion

        public static string SavesFilePath ="";
        public static string ZipFilePath = "";

        public static string ConnType = "";
        public static string ConnStr = "";

        public static string SendEmail = "";
        public static string SendPwd = "";

        public static string UserStoreName = "";
        public static string Suffix = "";
        public static string EmailReportName = "";

        public static string Temp_DayReprotSum = "";
        public static string EmailTitleDayReprotSum = "";
        public static string EmailDescDayReprotSum = "";
        public static string ServiceCode = "";
    }
}
