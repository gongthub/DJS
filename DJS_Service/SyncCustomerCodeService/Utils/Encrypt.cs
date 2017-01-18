using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OpsUtil
{
    public class Encrypt
    {
       
        //加密
        public static string EncryptDES(string value)
        {
            try
            {
                return System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(value));
            }
            catch(Exception e)
            {
                throw e;
            }
 
        }

        //解密
        public static string DecryptDES(string value)
        {
            try
            {
                return System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(value));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="srcValue"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string srcValue)
        {
            if (srcValue == null || srcValue == "") return null;
            System.Security.Cryptography.MD5 md = System.Security.Cryptography.MD5.Create();
            byte[] md2 = md.ComputeHash(System.Text.Encoding.UTF8.GetBytes(srcValue));
            System.Text.StringBuilder md3 = new System.Text.StringBuilder();
            for (int i = 0; i < md2.Length; i++)
            {
                md3.Append(md2[i].ToString("x2"));
            }
            return md3.ToString();
        }
    }
}
