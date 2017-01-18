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
        /// 写入日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        /// <param name="type">0:正常 1:错误</param>
        /// <param name="IsSendEmail">true:发送邮件 false:不发送邮件</param>
        void WriteLog(string msg, int type,bool IsSendEmail);

        /// <summary>
        /// 回写数据
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <param name="str">数据</param>
        void WirteDatas(string namespaces, string str);


        /// <summary>
        /// 根据任务名称查找配置文件名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetConfigNameByJobName(string jobName);

        /// <summary>
        /// 发送错误邮件提醒
        /// </summary> 
        /// <param name="str">错误信息</param>
        void SendErrorMail(string str);
    }
}
