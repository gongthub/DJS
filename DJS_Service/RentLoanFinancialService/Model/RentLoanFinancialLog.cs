using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class RentLoanFinancialLog
    {
        public int Id { get; set; }

        public string GUID { get; set; }

        public string CardNO { get; set; }

        public string CustomerCode { get; set; }

        public string ContractNO { get; set; }

        //0是放款，1是还款,2是逾期
        public int Type { get; set; }

        //原因
        public string Reason { get; set; }

        public DateTime CreateDate { get; set; }

        //Type为0时，表示放款日期，Typ为1时，表示还款时间
        public DateTime? LoanDate { get; set; }
        //是否成功
        public bool IsSuccess { get; set; }


    }
}
