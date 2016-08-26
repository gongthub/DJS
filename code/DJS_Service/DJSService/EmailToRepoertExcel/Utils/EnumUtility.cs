using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Utils
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
    }
}
