using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class SecurityHelp
    {

        #region 单例模式创建对象
        //单例模式创建对象
        private static SecurityHelp _securityHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        SecurityHelp()
        {
        }

        public static SecurityHelp securityHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _securityHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _securityHelp)
                        {
                            _securityHelp = new SecurityHelp();
                        }
                    }
                }
                return _securityHelp;
            }
        }
        #endregion

        #region 文件MD5 +string GetMD5HashFromFile(string fileName)
        /// <summary>
        /// 文件MD5
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogHelp.logHelp.WriteLogRedis("获取文件MD5错误:" + ex.Message, Model.Enums.LogType.Error);
                return "";
            }
        } 
        #endregion
    }
}
