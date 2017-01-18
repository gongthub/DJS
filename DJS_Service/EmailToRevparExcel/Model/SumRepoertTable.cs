using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRevparExcel.Model
{
    public class SumRepoertTable
    {
        public DateTime Date { get; set; }

        public int SubjectType { get; set; }

        public decimal SUMCoutOrAccount { get; set; }

        public int StoreID { get; set; }

    }

    public class AllSumRepoertTable
    {
        public DateTime Date { get; set; }

        public int SubjectType { get; set; }

        public decimal SUMCoutOrAccount { get; set; }
    }

    public class UserStore
    {
        public string StoreId { get; set; }

        public string TOUserEmail { get; set; }

        public string CCUserEmail { get; set; }

        public int Type { get; set; }
    }
}
