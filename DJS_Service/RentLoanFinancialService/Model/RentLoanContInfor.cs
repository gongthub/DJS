using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class RentLoanContInfor
    {
        public int ContractId { get; set; }

        public int ContractType { get; set; }

        public short ContractStatus { get; set; }

        public int StoreID { get; set; }

        public int RoomId { get; set; }

        public decimal CharterMoney { get; set; }

    }
}
