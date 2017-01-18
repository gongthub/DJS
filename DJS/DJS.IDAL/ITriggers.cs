using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.IDAL
{
    public interface ITriggers
    {
        /// <summary>
        /// 获取所有触发器
        /// </summary>
        /// <returns></returns>
        List<Model.Triggers> GetModels();
    }
}
