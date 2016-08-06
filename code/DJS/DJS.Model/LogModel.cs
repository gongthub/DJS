using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class LogModel
    {
        //主键
        public Guid ID { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public int Type { set; get; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeName { set; get; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { set; get; }
    }
}
