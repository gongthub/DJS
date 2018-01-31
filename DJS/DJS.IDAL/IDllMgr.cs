using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IDllMgr : IBaseMgr<DllMgr>
    {
        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns></returns>
        List<Model.DllMgr> GetModels();

        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExist(string name);
    }
}
