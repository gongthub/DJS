using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.SDK
{ 
    public interface IConfigMgr
    {
        /// <summary>
        /// 设置Config值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param> 
        /// <returns></returns>
        bool SetConfig(string name, string value);

        /// <summary>
        /// 设置Config值
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param> 
        /// <param name="msg">返回提示</param> 
        /// <returns></returns>
        bool SetConfig(string name, string value,out string msg);

        /// <summary>
        /// 根据名称查找值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetConfig(string name);

        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        bool IsExist(string name);
    }
}
