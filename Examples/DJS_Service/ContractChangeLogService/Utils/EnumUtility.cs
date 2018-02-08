using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractChangeLogService.Utils
{
    public class EnumUtility
    {
        public enum ContractStatus
        {
            [Description("待办合同")]
            待办 = 1,
            [Description("无效合同")]
            无效 = 2,
            [Description("执行中合同")]
            执行中 = 3,
            [Description("已完成合同")]
            过期 = 4,
            [Description("待退房")]
            待退房 = 5
        }
    }
}
