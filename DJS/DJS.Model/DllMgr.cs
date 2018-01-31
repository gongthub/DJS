using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class DllMgr : BaseEntity<DllMgr>
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
        /// 命名空间
        /// </summary>
        public string NameSpace { set; get; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { set; get; }
    }
}
