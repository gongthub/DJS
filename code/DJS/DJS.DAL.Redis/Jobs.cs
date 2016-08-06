using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.DAL.Redis
{
    public class Jobs : IDAL.IJobs
    {

        #region  获取所有jobs +List<Model.Jobs> GetJobs()
        /// <summary>
        /// 获取所有jobs
        /// </summary>
        /// <returns></returns>
        public List<Model.Jobs> GetModels()
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
