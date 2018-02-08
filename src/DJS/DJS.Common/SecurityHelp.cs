using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class SecurityHelp
    {
        private object lockObj = new object();  

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
            lock (lockObj)
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
                        file.Dispose();
                        file.Close();
                        return sb.ToString();
                    }
                }
                catch (Exception ex)
                {
                    LogHelp.logHelp.WriteLog("获取文件MD5错误:" + ex.Message, Enums.LogType.Error);
                    //System.Threading.Thread.Sleep(1000);
                    return "";
                }
            }
        }
        #endregion

        #region 文件MD5 +string GetMD5HashFromFile(string fileName)
        /// <summary>
        /// 文件MD5
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public byte[] GetMD5Hash(string fileName)
        {
            lock (lockObj)
            {
                try
                {
                    using (FileStream file = new FileStream(fileName, FileMode.Open))
                    {
                        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] retVal = md5.ComputeHash(file);

                        return retVal;
                    }
                }
                catch (Exception ex)
                {
                    LogHelp.logHelp.WriteLog("获取文件MD5错误:" + ex.Message, Enums.LogType.Error);
                    //System.Threading.Thread.Sleep(1000);
                    return null;
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

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="code">加密位数16/32</param>
        /// <returns></returns>
        public static string md5(string str, int code)
        {
            string strEncrypt = string.Empty;
            if (code == 16)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
            }

            if (code == 32)
            {
                strEncrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            }

            return strEncrypt;
        }

        private static string DESKey = "djs_web";

        #region ========加密========
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, DESKey);
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                return Decrypt(Text, DESKey);
            }
            else
            {
                return "";
            }
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}
