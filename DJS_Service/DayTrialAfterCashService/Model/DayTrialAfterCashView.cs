using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayTrialAfterCashService.Model
{
    public class DayTrialAfterCashView
    {
        public int ID { get; set; }

        public int StoreID { get; set; }

        public string StoreCode { get; set; }

        public string StoreName { get; set; }

        public string StoreAddress { get; set; }

        public DateTime? Date { get; set; }

        public decimal RemainingCash { get; set; }

        public DateTime? UpSaveCashDate { get; set; }
    }
}
