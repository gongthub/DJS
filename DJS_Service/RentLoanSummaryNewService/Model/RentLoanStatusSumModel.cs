using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryNewService.Model
{
    public class RentLoanStatusSumModel
    {
        public int StoreID { get; set; }

        public DateTime Date { get; set; }

        /// <summary>
        /// 租金贷状态
        /// </summary>
        public int RentLoanStatus { get; set; }

        /// <summary>
        /// 未申请租金贷
        /// </summary>
        public int NotApplyCount { get; set; }

        /// <summary>
        /// 待提交
        /// </summary>
        public int WaitSubmitCount { get; set; }

        /// <summary>
        /// 待审核
        /// </summary>
        public int WaitAuditCount { get; set; }

        /// <summary>
        /// 审核未通过
        /// </summary>
        public int AuditNotCrossCount { get; set; }

        /// <summary>
        /// 通过未放款
        /// </summary>
        public int CrossNotLoan { get; set; }

        /// <summary>
        /// 已放款
        /// </summary>
        public int AlreadyLoan { get; set; }

        /// <summary>
        /// 正常还款
        /// </summary>
        public int NormalRepayment { get; set; }

        /// <summary>
        /// 逾期未还款
        /// </summary>
        public int OverdueNotRepayment { get; set; }
    }
}
