using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractWarningSMS.Model
{
    public class ContractExpireForServiceView
    {
        public int ID { get; set; }

        public int StoreID { get; set; }

        public string StoreName { get; set; }

        public int RoomID { get; set; }

        public string FullName { get; set; }

        public int RenterID { get; set; } 
        public string CustomerCode { get; set; }
        public string RenterName { get; set; }

        public string Phone { get; set; }

        public string ContractNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ContractType { get; set; }

        public int ExpireDay { get; set; }
    }
}
