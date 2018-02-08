using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryService.Model
{
    public class EmailCategory
    {
        public int ID { get; set; }

        public int CategoryID { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
