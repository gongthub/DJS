using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyRentApportionService.Models
{
   public class MonthRentApportion
    {
        public int ContractID { get; set; }

        public int RoomID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public decimal RealAmount { get; set; }

        public decimal Amount { get; set; }

        public decimal RsvAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        public int? PeriodicChargeID { get;set; }

        public DateTime Date { get; set; }
    }
}
