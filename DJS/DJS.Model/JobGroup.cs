using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class JobGroup : BaseEntity<JobGroup>
    {

        /// <summary>
        /// 序号
        /// </summary>
        public int SortCode { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 任务数量
        /// </summary>
        public int JobNum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Description { set; get; }

        public bool? EnabledMark { get; set; }
    }
}
