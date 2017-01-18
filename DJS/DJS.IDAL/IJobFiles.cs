using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IJobFiles
    {
        /// <summary>
        /// 获取jobs
        /// </summary>
        /// <returns></returns>
        List<JobFiles> GetModels();

        /// <summary>
        /// 根据id获取job
        /// </summary>
        /// <returns></returns>
        JobFiles GetModelById(Guid Id);

        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExist(string name);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(Model.JobFiles model);

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DelById(Guid Id);

        /// <summary>
        /// 根据JobId删除
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        bool DelByJobId(Guid JobId);

        /// <summary>
        /// 根据JobName删除
        /// </summary>
        /// <param name="JobName"></param>
        /// <returns></returns>
        bool DelByJobName(string JobName);
    }
}
