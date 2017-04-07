using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryNewService.Model
{
    public class RentLoanSummary
    {
        public int ID { get; set; }

        public int StoreID { get; set; }

        public DateTime Date { get; set; }

        public int RentLoanStatus { get; set; }

        public int ContractCount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
