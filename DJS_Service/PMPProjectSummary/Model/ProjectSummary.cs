using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Model
{
   public class ProjectSummary
    {

       public int ID { get; set; }

       /// <summary>
        /// 城市
       /// </summary>
       public string CityName { get; set; }
       /// <summary>
       /// 性质
       /// </summary>
       public string ItemType { get; set; }
       /// <summary>
       /// 项目编号
       /// </summary>
       public string ItemCode { get; set; }
       /// <summary>
       /// 项目名称
       /// </summary>
       public string ItemName { get; set; }

       public int? Status { get; set; }
       /// <summary>
       /// 项目分期
       /// </summary>
       public string ProjectName { get; set; }
       /// <summary>
       /// 分期房间数
       /// </summary>
       public int? ProjectRoom { get; set; }

       /// <summary>
       /// 分期床位数
       /// </summary>
       public int? BedNumber { get; set; }
       /// <summary>
       /// 周期不可售房间数
       /// </summary>
       public int? NotSellRoom { get; set; }
       /// <summary>
       /// 周期不可售床位数
       /// </summary>

       public int? NotSellBedNumber { get; set; }



       /// <summary>
       /// 项目周期状态
       /// </summary>
       public string ProjectCycleState { get; set; }
       /// <summary>
       /// 项目节点状态
       /// </summary>
       public string projectDetailName { get; set; }
       /// <summary>
       /// 项目周期类别
       /// </summary>
       public string ProjectType { get; set; }
       /// <summary>
       /// 周期计划开始时间
       /// </summary>
       public DateTime? StartDate { get; set; }
       /// <summary>
       /// 周期计划完工时间
       /// </summary>
       public DateTime EndDate { get; set; }
       /// <summary>
       /// 周期实际开始时间
       /// </summary>
       public DateTime? RealStartDate { get; set; }
       /// <summary>
       /// 周期实际结束时间
       /// </summary>
       public DateTime? RealEndDate { get; set; }
       /// <summary>
       /// 项目完成度
       /// </summary>
       public string ProjectCompletion { get; set; }
       /// <summary>
       /// 延迟原因汇总
       /// </summary>
       public string ReasonDelaySummary { get; set; }
    }
}
