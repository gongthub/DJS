using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPSmallNodesExtensionSummary.Model
{
    public   class SmallNodesExtensionModel
    {

        /// <summary>
        /// 项目经理
        /// </summary>
        public int ManagerID { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 工期名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 大节点名称
        /// </summary>
        public string ModelDetailName { get; set; }
        /// <summary>
        /// 小节点名称
        /// </summary>
        public string ModelNodeName { get; set; }
        /// <summary>
        /// 小节点计划开始时间
        /// </summary>
        public DateTime ProjectNodeStartDate { get; set; }
        /// <summary>
        /// 小节点计划结束时间
        /// </summary>
        public DateTime ProjectNodeEndDate { get; set; }
        /// <summary>
        /// 小节点实际开始时间
        /// </summary>
        public DateTime? ProjectNodesRealStartDate { get; set; }
        /// <summary>
        /// 小节点实际结束时间
        /// </summary>
        public DateTime? ProjectNodesRealEndDate { get; set; }
        /// <summary>
        /// 延迟类别
        /// </summary>
        public string DelayReason { get; set; }
        /// <summary>
        /// 延时天数
        /// </summary>
        public int DelayDate { get; set; }
    }
}
