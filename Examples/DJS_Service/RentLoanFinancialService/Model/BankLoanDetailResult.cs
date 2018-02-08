using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class BankLoanDetailResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public bool IsResult { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public List<LoanDetailResult> Data { get; set; }
    }

    public class LoanDetailResult{
        public int ID{get;set;}

        public string GUID{get;set;}

        public string ContractNO{get;set;}

        public string BankCode{get;set;}

        public string CustomerCode{get;set;}

        public string CustomerName{get;set;}

        public string CardNO{get;set;}

        public int LoanPeriods{get;set;}

        public decimal LoanTotalMoney{get;set;}

        public DateTime LoanDate{get;set;}

        public int StatementDay{get;set;}

        public string SourceCode{get;set;}

        public DateTime CREATED_DATE{get;set;}

        public DateTime LAST_UPD_DATE{get;set;}


    }
}
