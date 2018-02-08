using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Model
{
    public class Project
    {
        public int ID { get; set; }

        /// <summary>
        /// 工程项目
        /// </summary>
        public int? ItemID { get; set; }
      
        /// <summary>
        /// 工程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 预计结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStartDate { get; set; }

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RealEndDate { get; set; }

        /// <summary>
        /// 预计完工时间
        /// </summary>
        public DateTime? ExpectedTime { get; set; }
        public int? Type { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        public int IsLead { get; set; }

        public string Address { get; set; }
        public string ManagerPhone { get; set; }
        public int? Room { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public int? ManagerID { get; set; }

        /// <summary>
        /// 模版
        /// </summary>
        public int? ModelID { get; set; }
        //public virtual Model Model { get; set; }

        public DateTime? CreateDate { get; set; }
        public int? CreateUserID { get; set; }

        public int? RMID { get; set; }
        //[ForeignKey("RMID")]
        //public virtual UserProfile RM { get; set; }
    }
}
