using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IJobFiles : IBaseMgr<JobFiles>
    {

        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExist(string name, string jobID);

        /// <summary>
        /// 根据Name删除
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        bool DeleteByName(string name);
        /// <summary>
        /// 根据JobId删除
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        bool DelByJobId(string JobId);

        /// <summary>
        /// 根据JobName删除
        /// </summary>
        /// <param name="JobName"></param>
        /// <returns></returns>
        bool DelByJobName(string JobName);
        /// <summary>
        /// 根据任务ID获取文件
        /// </summary>
        /// <returns></returns>
        List<JobFiles> GetModels(string jobId);
    }
}
