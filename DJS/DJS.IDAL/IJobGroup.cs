using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IJobGroup
    {
        /// <summary>
        /// 获取任务组数据集合
        /// </summary>
        /// <returns></returns>
        List<Model.JobGroup> GetModels();

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
        bool Add(Model.JobGroup  model);

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DelById(Guid Id);

    }
}
