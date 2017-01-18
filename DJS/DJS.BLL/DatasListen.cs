using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class DatasListen
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DatasListen()
        {
            //初始化时间线程
            _timer = new Timer(new TimerCallback(DatasCountListen), null, 0, _checkInterval);
 
        }
         

        /// <summary>
        /// 上一个文件MD5值
        /// </summary>
        public static string LastMD5 = "";

        /// <summary>
        /// 数据文件路径
        /// </summary>
        public static string PATH = "";

        /// <summary>
        /// 定义检测时间间隔
        /// </summary>
        private static readonly int _checkInterval = 1000;

        /// <summary>
        /// 检测信息监听的时间对象
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// 此委托类型的事件
        /// </summary>
        public event EventHandler<EventArgs> OnChange_ListenDatas;

        /// <summary>
        /// 定义Datas改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void AddDatasChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenDatas == null)
            {

                OnChange_ListenDatas = new EventHandler<EventArgs>(hander);
            }
            else
            {
                OnChange_ListenDatas += hander;
            }
        }


        /// <summary>
        /// 删除Datas改变事件订阅方法
        /// </summary>
        /// <param name="hander"></param>
        public void DelJobsChangeHandler(EventHandler<EventArgs> hander)
        {
            //判断事件是否为null
            if (OnChange_ListenDatas == null)
            {

                OnChange_ListenDatas = new EventHandler<EventArgs>(hander);
            }
            else
            { 
                OnChange_ListenDatas -= hander;  
            }
        }


        /// <summary>
        /// 定义监测Datas是否发生改变
        /// </summary>
        /// <param name="sender"></param>
        private void DatasCountListen(object sender)
        {
            if (PATH != "" && Common.FileHelp.FileExists(PATH))
            {
                string mdnow = Common.SecurityHelp.securityHelp.GetMD5HashFromFile(PATH);
                if (mdnow != LastMD5)
                {
                    if (OnChange_ListenDatas != null)
                    {
                        LastMD5 = mdnow;
                        EventArgs arg = new EventArgs();
                        OnChange_ListenDatas(this, arg);
                    }
                }
            }
        }

    }
}
