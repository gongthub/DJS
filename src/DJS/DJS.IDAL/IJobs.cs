using DJS.Common;
using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IJobs : IBaseMgr<Jobs>
    {
        /// <summary>
        /// 获取jobs
        /// </summary>
        /// <returns></returns>
        List<Jobs> GetModels();

        /// <summary>
        /// 获取job
        /// </summary>
        /// <returns></returns>
        Model.Jobs GetFormByName(string keyValue);

        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExist(string name);

        /// <summary>
        /// 获取任务配置信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        List<SelectStrLists> GetConfigs(string keyValue);

        /// <summary>
        /// 保存任务配置信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="selConfigs"></param>
        /// <returns></returns>
        bool SaveConfigs(string keyValue, List<SelectStrLists> selConfigs);
        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool RemoveConfigs(string keyValue);

    }
}
