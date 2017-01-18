using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryService.Model
{
    public class EmailLog
    {
        public int ID { get; set; }

    
        public string UserID { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }

        public int StoreID { get; set; }
        public int Type { get; set; }

        public int CategoryID { get; set; }

        public string Message { get; set; }
    }
}
