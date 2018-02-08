using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentWarningSMS.Model
{
    public class RentExpireView
    {
        public int ID { get; set; }

        public int StoreID { get; set; }

        public string StoreName { get; set; }
          
        public string FullName { get; set; }

        public string CustomerCode { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }

        public string ContractNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
          
        public int ExpireDay { get; set; }
    }
}
