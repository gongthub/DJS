using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class RentLoanPoolResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public bool IsResult { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public List<LoanPoolResult> Data { get; set; }
    }
    public class LoanPoolResult
    {
        public int ID { get; set; }

        public string GUID { get; set; }

        public string ContractNO { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CardNO { get; set; }

        public int LoanPeriods { get; set; }

        public decimal LoanMoney { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime StatementDay { get; set; }

        public string SourceCode { get; set; }

        public DateTime CREATED_DATE { get; set; }

        public DateTime LAST_UPD_DATE { get; set; }
    }
}
