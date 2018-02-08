using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.SDK
{
    public interface IDJSClient : IJob
    {
        /// <summary>
        /// 根据名称设置配置信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        void SetConfig(string jobName);
    }
}
