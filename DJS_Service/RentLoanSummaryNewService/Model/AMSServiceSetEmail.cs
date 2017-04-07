using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanSummaryNewService.Model
{
    public class AMSServiceSetEmail
    {
        public int ID { get; set; }

        public int AMSServiceSetID { get; set; }

        public string StoreID { get; set; }

        public string TOPeople { get; set; }

        public string CCPeople { get; set; }

        public bool IsSum { get; set; }

        private int _Status = 1;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
    }
}
