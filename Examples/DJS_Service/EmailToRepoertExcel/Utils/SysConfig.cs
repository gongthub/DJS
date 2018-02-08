
using System;
using System.IO;
namespace EmailToRepoertExcel.Utils
{
    public class SysConfig
    {
        private static IniFile iniFile;
        private static SysConfig sysConfig = new SysConfig();   

        #region 字段


        private string connType;
        private string connStr;
        private bool isSendRevpar;

        private string savesFilePath;
        private string zipFilePath;
        private string sendToEmail;
        private string sendToCCEmail;
        private string sendEmail;
        private string sendPwd;

        private string everyDayRunHour;
        private string everyDayRunMinute;
   
        private string templatePath;
        private string revparTemplatePath;
        private int interval;

        private string emailTitle;
        private string emailDesc;
        private string suffix;

        private string emailTitleRevpar;
        private string emailDescRevpar;

        private string sqlPath;
        private string dayRepoertFileName;
        private string storeFileName;
        private string userStoreName;
        private string searchStore;
        private string searchAll;
        #endregion



        private int ErrCode;                //返回的错误号 , 正确是0，其他都是错误
        private string ErrMsg;              //返回的错误信息
        private string IniFileName;         //Ini文件名称
        private string AppName;             //应用程序名称

        public int GetErrCode() { return ErrCode; }
        public string GetErrMsg() { return ErrMsg; }
        public string GetAppName() { return AppName; }




        //private SysConfig()
        //{
        //    ReadConfig();
        //}

        public static SysConfig Instance()
        {
            return sysConfig;
        }


        public SysConfig(string FileName)
		{
			ErrCode = -1;
			ErrMsg = "";
			IniFileName = FileName;
            iniFile = new IniFile(IniFileName, false);
			AppName = "报表邮件发送模块";
		}

        public SysConfig()
		{
            //IniFileName = System.Environment.CurrentDirectory + "SysConfig.ini";
			ErrCode = -1;
			ErrMsg = "";
            IniFileName = "SysConfig.ini";
            iniFile = new IniFile(IniFileName, false);
            AppName = "报表邮件发送模块";
            ReadConfig();
		}

      


        private void ReadConfig()
        {
            try
            {

                if (File.Exists(IniFileName))
                {

                    ConnType = iniFile.ReadString("DbConfig", "ConnType", "");
                    ConnStr = iniFile.ReadString("DbConfig", "ConnStr", "");
                    IsSendRevpar = "true".Equals(iniFile.ReadString("DbConfig", "IsSendRevpar", ""), System.StringComparison.OrdinalIgnoreCase);

                    SavesFilePath = iniFile.ReadString("FilePath", "SavesFilePath", "");
                    ZipFilePath = iniFile.ReadString("FilePath", "ZipFilePath", "");

                    SendToEmail = iniFile.ReadString("SendEmail", "SendToEmail", "");
                    SendToCCEmail = iniFile.ReadString("SendEmail", "SendToCCEmail", "");
                    SendEmail = iniFile.ReadString("SendEmail", "SendEmail", "");
                    SendPwd = iniFile.ReadString("SendEmail", "SendPwd", "");
                    EmailTitle = iniFile.ReadString("SendEmail", "EmailTitle", "");
                    EmailDesc = iniFile.ReadString("SendEmail", "EmailDesc", "");
                    EmailTitleRevpar = iniFile.ReadString("SendEmail", "EmailTitleRevpar", "");
                    EmailDescRevpar = iniFile.ReadString("SendEmail", "EmailDescRevpar", "");

                    EveryDayRunHour = iniFile.ReadString("ThreShold", "EveryDayRunHour", "");
                    EveryDayRunMinute = iniFile.ReadString("ThreShold", "EveryDayRunMinute", "");

                    TemplatePath = iniFile.ReadString("BasicParameter", "TemplatePath", "");
                    RevparTemplatePath = iniFile.ReadString("BasicParameter", "RevparTemplatePath", "");
                    Interval = iniFile.ReadInteger("BasicParameter", "Interval", 0);
                    Suffix = iniFile.ReadString("BasicParameter", "Suffix", "");


                    SqlPath = iniFile.ReadString("SqlParameter", "SqlPath", "");
                    DayRepoertFileName = iniFile.ReadString("SqlParameter", "DayRepoertFileName", "");
                    StoreFileName = iniFile.ReadString("SqlParameter", "StoreFileName", "");
                    UserStoreName = iniFile.ReadString("SqlParameter", "UserStoreName", "");
                    SearchStore = iniFile.ReadString("SqlParameter", "SearchStoreFileName", "");
                    SearchAll = iniFile.ReadString("SqlParameter", "SearchAllFileName", "");
                }
            }
            catch (Exception ex)
            {
                Log.Instance().Info("读取配置文件出错，原因"+ex.Message);
                //LogHelper.Always("读取配置文件出错，原因：" + ex.Message);
            }
        }


        public bool ReadIniFile()
        {
            try
            {
                if (File.Exists(IniFileName))
                {
                    ConnType = iniFile.ReadString("DbConfig", "ConnType", "");
                    ConnStr = iniFile.ReadString("DbConfig", "ConnStr", "");

                    SavesFilePath = iniFile.ReadString("FilePath", "SavesFilePath", "");
                    ZipFilePath = iniFile.ReadString("FilePath", "ZipFilePath", "");

                    SendToEmail = iniFile.ReadString("SendEmail", "SendToEmail", "");
                    SendToCCEmail = iniFile.ReadString("SendEmail", "SendToCCEmail", "");
                    SendEmail = iniFile.ReadString("SendEmail", "SendEmail", "");
                    SendPwd = iniFile.ReadString("SendEmail", "SendPwd", "");
                    EmailTitle = iniFile.ReadString("SendEmail", "EmailTitle", "");
                    EmailDesc = iniFile.ReadString("SendEmail", "EmailDesc", "");
                    EmailTitleRevpar = iniFile.ReadString("SendEmail", "EmailTitleRevpar", "");
                    EmailDescRevpar = iniFile.ReadString("SendEmail", "EmailDescRevpar", "");

                    EveryDayRunHour = iniFile.ReadString("ThreShold", "EveryDayRunHour", "");
                    EveryDayRunMinute = iniFile.ReadString("ThreShold", "EveryDayRunMinute", "");

                    TemplatePath = iniFile.ReadString("BasicParameter", "TemplatePath", "");
                    RevparTemplatePath = iniFile.ReadString("BasicParameter", "RevparTemplatePath", "");
                    Interval = iniFile.ReadInteger("BasicParameter", "Interval", 0);
                    Suffix = iniFile.ReadString("BasicParameter", "Suffix", "");

                    SqlPath = iniFile.ReadString("SqlParameter", "SqlPath", "");
                    DayRepoertFileName = iniFile.ReadString("SqlParameter","DayRepoertFileName","");
                    StoreFileName = iniFile.ReadString("SqlParameter","StoreFileName","");
                    UserStoreName = iniFile.ReadString("SqlParameter", "UserStoreName", "");
                    SearchStore = iniFile.ReadString("SqlParameter", "SearchStoreFileName", "");
                    SearchAll = iniFile.ReadString("SqlParameter", "SearchAllFileName", "");
                }
            }
            catch (Exception e)
            {
                ErrCode = -1;
                ErrMsg = "读取配置文件[" + IniFileName + "]失败, 失败原因：" + e.Message;
                return false;
            }

            ErrCode = 0;
            ErrMsg = "读取配置文件成功";
            return true;
        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool WriteIniFile(string Session, string Key, string Value)
        {
            try
            {
                iniFile.WriteString(Session, Key, Value);
            }
            catch
            {
                ErrCode = -1;
                ErrMsg = "写入到配置文件[" + IniFileName + "]中的Session[" + Session + "]的Key值[" + Key + "]失败";
                return false;
            }

            ErrCode = 0;
            ErrMsg = "写入到配置文件[" + IniFileName + "]中的Session[" + Session + "]的Key值[" + Key + "]成功";
            return true;
        }

        public string ZipFilePath
        {
            get { return zipFilePath; }
            set { zipFilePath = value; }        
        }

        public string SavesFilePath
        {
            get { return savesFilePath; }
            set { savesFilePath = value; }
        }

        public string SendToEmail
        {
            get { return sendToEmail; }
            set { sendToEmail = value; }
        }

        public string EmailTitle
        {
            get { return emailTitle; }
            set { emailTitle = value; }
        }

        public string EmailDesc
        {
            get { return emailDesc; }
            set { emailDesc = value; }
        }

        public string EmailTitleRevpar
        {
            get { return emailTitleRevpar; }
            set { emailTitleRevpar = value; }
        }

        public string EmailDescRevpar
        {
            get { return emailDescRevpar; }
            set { emailDescRevpar = value; }
        }

        public string  SendToCCEmail
        {
            get { return sendToCCEmail; }
            set { sendToCCEmail = value; }
        }

        public string SendEmail
        {
            get { return sendEmail; }
            set { sendEmail = value; }
        }

        public string SendPwd
        {
            get { return sendPwd; }
            set { sendPwd = value; }
        }

        public string ConnType
        {
            get { return connType; }
            set { connType = value; }
        }

        public string ConnStr
        {
            get { return connStr; }
            set { connStr = value; }
        }

        public bool IsSendRevpar
        {
            get { return isSendRevpar; }
            set { isSendRevpar = value; }
        }

        public string EveryDayRunHour
        {
            get { return everyDayRunHour; }
            set { everyDayRunHour = value; }
        }

        public string  EveryDayRunMinute
        {
            get { return everyDayRunMinute; }
            set { everyDayRunMinute = value; }
        }
      
       public string  TemplatePath
       {
           get { return templatePath; }
           set { templatePath = value; }       
       }

       public string RevparTemplatePath
       {
           get { return revparTemplatePath; }
           set { revparTemplatePath = value; }       
       }

        public string SqlPath
        {
            get { return sqlPath; }
            set { sqlPath = value; }        
        }

        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        public string Suffix
        {
            get { return suffix; }
            set { suffix = value; }
        }

        public string DayRepoertFileName
        {
            get { return dayRepoertFileName; }
            set { dayRepoertFileName = value; }
        }

        public string StoreFileName
        {
            get { return storeFileName; }
            set { storeFileName = value; }        
        }

        public string UserStoreName
        {
            get { return userStoreName; }
            set { userStoreName = value; }
        }



        public string SearchStore
        {
            get { return searchStore; }
            set { searchStore = value; }
        }


        public string SearchAll
        {
            get { return searchAll; }
            set { searchAll = value; }
        }
    }
}
