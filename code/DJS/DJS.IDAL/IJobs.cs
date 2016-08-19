using DJS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface IJobs
    {
        /// <summary>
        /// 获取jobs
        /// </summary>
        /// <returns></returns>
        List<Jobs> GetModels();


        /// <summary>
        /// 根据id获取job
        /// </summary>
        /// <returns></returns>
        Jobs GetModelById();

    }
}
