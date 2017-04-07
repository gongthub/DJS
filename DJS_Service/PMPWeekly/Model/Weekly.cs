using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPWeekly.Model
{
    /// <summary>
    /// 周报
    /// </summary>
    public  class Weekly
    {

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 工期名词
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 施工单位
        /// </summary>
        public string Builder { get; set; }
        /// <summary>
        /// 主要风险点
        /// </summary>
        public string Risk { get; set; }
        /// <summary>
        /// 本周项目形象进度
        /// </summary>
        public string LayoutProcess { get; set; }
        /// <summary>
        /// 项目主要工程事项（施工单位工作）
        /// </summary>
        public string WeekProjectItems { get; set; }
        /// <summary>
        /// 本周项目工作完成情况
        /// </summary>
        public string CompleteSituation { get; set; }
        /// <summary>
        /// 未完成原因及修改措施
        /// </summary>
        public string DelayReason { get; set; }
        /// <summary>
        /// 下周主要工程事项
        /// </summary>
        public string NextWeekContent { get; set; }
        /// <summary>
        /// 下周工作计划
        /// </summary>
        public string NextWeekPlan { get; set; }
        /// <summary>
        /// 计划完成时间
        /// </summary>
        public string PlanCompleteDate { get; set; }
        /// <summary>
        /// 项目阶段验收情况
        /// </summary>
        public string AcceptanceSituation { get; set; }
        /// <summary>
        /// 签约总造价及付款情况
        /// </summary>
        public string PaymentContent { get; set; }
        /// <summary>
        /// 消防、报建手续取证计划
        /// </summary>
        public string FirePlan { get; set; }
        /// <summary>
        /// 预算外成本增加记录
        /// </summary>
        public string BudgetRecord { get; set; }
        /// <summary>
        /// 施工现场人员数量
        /// </summary>
        public string WorkPeoples { get; set; }
        /// <summary>
        /// 甲供材料到货情况
        /// </summary>
        public string AMaterialPresent { get; set; }
        /// <summary>
        /// 乙供材料到货情况
        /// </summary>
        public string BMaterialPresent { get; set; }
        /// <summary>
        /// 对该项目建议
        /// </summary>
        public string ProjectOpinion { get; set; }


    }
}
