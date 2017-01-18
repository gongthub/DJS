using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
   public class NetHelp
    {
        #region 单例模式创建对象
        //单例模式创建对象
        private static NetHelp _netHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        NetHelp()
        {
        }

        public static NetHelp netHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _netHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _netHelp)
                        {
                            _netHelp = new NetHelp();
                        }
                    }
                }
                return _netHelp;
            }
        }
        #endregion


        #region 获取本地ipv4地址
        /// <summary>
        /// 获取本地ipv4地址
        /// </summary>
        /// <returns></returns>
        public List<string> GetIP()
        {
            List<string> ipAddrs = new List<string>();
            string hostName = Dns.GetHostName();//本机名   
            //System.Net.IPAddress[] addressList = Dns.GetHostByName(hostName).AddressList;//会警告GetHostByName()已过期，我运行时且只返回了一个IPv4的地址   
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) //判断是否IPv4
                    ipAddrs.Add(ip.ToString());
            }

            return ipAddrs;
        }
        #endregion
    }
}
