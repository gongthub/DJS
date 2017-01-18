using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.ContractSpace
{
    public  class TempCharge
    {
        public decimal? EarlyCharge { get; set; }

        public decimal? ThisMonthReceivable { get; set; }

        public decimal? ThisMonthPaidIn { get; set; }


        public decimal? ThisMonthUncollected { get; set; }



    }
}
