using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryNewService.Utils
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
            //邮件发送邮箱
            SendMailSender = iConfigMgr.GetConfig("SendMailSender");
            SendMailPassword = iConfigMgr.GetConfig("SendMailPassword");         
            EmailSMTP = iConfigMgr.GetConfig("EmailSMTP");
            IDBEmailService = iConfigMgr.GetConfig("IDBRepositoryEmailService");

            ServiceCode = iConfigMgr.GetConfig("ServiceCode");
            SystemName = iConfigMgr.GetConfig("SystemName");

            System = iConfigMgr.GetConfig("System");

            WarningDate = iConfigMgr.GetConfig("WarningDate");
            MinWarningDate = iConfigMgr.GetConfig("MinWarningDate");

            //附件相关
            SavesFilePath = iConfigMgr.GetConfig("SavesFilePath");
            ZipFilePath = iConfigMgr.GetConfig("ZipFilePath");
            Temp_RentLoan = iConfigMgr.GetConfig("Temp_RentLoan");
            Suffix = iConfigMgr.GetConfig("Suffix"); 
        }

        #endregion

        //邮件发送邮箱
        public static string SendMailSender = "";
        public static string SendMailPassword = "";
        //邮件发送服务
        public static string EmailSMTP = "";
        public static string IDBEmailService = "";
        public static string ServiceCode = "";
        public static string SystemName = "";
        public static string System = "";

        public static string WarningDate = "";          //预警日期 45天
        public static string MinWarningDate = "";       //预警日期 5天    

        //附件相关
        public static string SavesFilePath = "";
        public static string ZipFilePath = "";
        public static string Temp_RentLoan = "";
        public static string Suffix = ""; 
    }
}
