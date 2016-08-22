using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.SDK
{
    public interface ILog
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="type">0:正常 1:错误</param>
        void WriteLog(string msg, int type);

        /// <summary>
        /// 回写数据
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <param name="str">数据</param>
        void WirteDatas(string namespaces, string str);
    }
}
