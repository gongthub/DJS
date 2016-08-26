using Microsoft.Win32;
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
            lock (this)
            {
                try
                {
                    using (FileStream file = new FileStream(fileName, FileMode.Open))
                    {
                        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] retVal = md5.ComputeHash(file);
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < retVal.Length; i++)
                        {
                            sb.Append(retVal[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                }
                catch (Exception ex)
                {
                    LogHelp.logHelp.WriteLog("获取文件MD5错误:" + ex.Message, Model.Enums.LogType.Error);
                    return "";
                }
            }
        }
        #endregion

        #region 设置应用程序开机自动运行 +string SetAutoRun(string fileName, bool isAutoRun)
        /// <summary>
        /// 设置应用程序开机自动运行
        /// </summary>
        /// <param name="fileName">应用程序的文件名</param>
        /// <param name="isAutoRun">是否自动运行,为false时，取消自动运行</param>
        /// <exception cref="system.Exception">设置不成功时抛出异常</exception>
        /// <returns>返回1成功，非1不成功</returns>
        public string SetAutoRun(string fileName, bool isAutoRun)
        {
            string reSet = string.Empty;
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    reSet = "该文件不存在!";
                }
                string name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                {
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                }
                if (isAutoRun)
                {
                    reg.SetValue(name, fileName);
                    reSet = "1";
                }
                else
                {
                    reg.SetValue(name, false);
                }

            }
            catch (Exception ex)
            {
                LogHelp.logHelp.WriteLog(ex.Message, 1);
            }
            finally
            {
                if (reg != null)
                {
                    reg.Close();
                }
            }
            return reSet;
        }
        #endregion
         
        #region 判断应用程序是否开机自动运行 +bool AppIsAutoRun(string fileName)
        /// <summary>
        /// 判断应用程序是否开机自动运行
        /// </summary>
        /// <param name="fileName">应用程序的文件名</param>
        /// <returns></returns>
        public bool AppIsAutoRun(string fileName)
        {
            bool ret = true;
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    ret = false;
                }
                string name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                {
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                }
                object obj = reg.GetValue(name);
                bool objret = false;
                if (obj != null)
                {
                    bool.TryParse(obj.ToString(), out objret);
                }
                if (obj != null && objret != false)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }

            }
            catch (Exception ex)
            {
                LogHelp.logHelp.WriteLog(ex.Message, 1);
            }
            finally
            {
                if (reg != null)
                {
                    reg.Close();
                }
            }
            return ret;
        }
        #endregion
    }
}
