using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Utils
{
    public class EnumUtility
    {
        /// <summary>
        /// 发送类型0:邮件 1:短信
        /// </summary>
        public enum EmialModel
        {
            Emial = 0,
            SMS = 1
        }

        public enum EmialStatu
        {
            主送 = 1,
            抄送 = 2,
            短信=3
        }

        public enum EmailCategory
        { 
            租金贷预警=1,
            合同到期预警=2,
            租金收费预警=3,
            宽带收费预警=4
        }

        public enum EmailLogStatu
        {
            成功 = 1,
            失败 = 2
        }

        /// <summary>
        /// 租金贷审核状态
        /// </summary>
        public enum RLoanStatus
        {
            [Description("待提交")]
            待提交 = 0,
            [Description("待审核")]
            待审核 = 1,
            [Description("审核通过")]
            审核通过 = 2,
            [Description("审核未通过")]
            审核未通过 = 3,
            [Description("已放款")]
            已放款 = 4,
            [Description("正常还款")]
            正常还款 = 5,
            [Description("正常终止")]
            正常终止 = 6,
            [Description("异常终止")]
            异常终止 = 7,
            [Description("逾期未还款")]
            逾期未还款 = 9,
            [Description("未申请租金贷")]
            未申请租金贷 = 100
        }
    }
}
