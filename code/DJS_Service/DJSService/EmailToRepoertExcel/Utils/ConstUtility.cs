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
            //iConfigMgr = DJS.SDK.DataAccess.CreateIConfigMgr();
        }

        #endregion
        public static string SavesFilePath = iConfigMgr.GetConfig("SavesFilePath");
        public static string ZipFilePath = iConfigMgr.GetConfig("ZipFilePath");

        public static string ConnType = iConfigMgr.GetConfig("ConnType");
        public static string ConnStr = iConfigMgr.GetConfig("ConnStr"); 

        public static string SendEmail = iConfigMgr.GetConfig("SendEmail");
        public static string SendPwd = iConfigMgr.GetConfig("SendPwd");

        public static string Temp_Revpar = iConfigMgr.GetConfig("Temp_Revpar");
        public static string Temp_DaylReportSum = iConfigMgr.GetConfig("Temp_DaylReportSum");

        public static string StoreFileName = iConfigMgr.GetConfig("StoreFileName"); 
        public static string DayRepoertFileName = iConfigMgr.GetConfig("DayRepoertFileName");
        public static string UserStoreName = iConfigMgr.GetConfig("UserStoreName");
        public static string SearchAll = iConfigMgr.GetConfig("SearchAll");
        public static string SearchStore = iConfigMgr.GetConfig("SearchStore");
        public static string Suffix = iConfigMgr.GetConfig("Suffix");
        public static string EmailTitle = iConfigMgr.GetConfig("EmailTitle");
        public static string EmailDesc = iConfigMgr.GetConfig("EmailDesc");



    }
}
