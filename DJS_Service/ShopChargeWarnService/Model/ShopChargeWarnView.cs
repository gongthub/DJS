using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopChargeWarnService.Model
{
    public class ShopChargeWarnView
    {
        public int ID { get; set; }

        public int StoreID { get; set; }

        public string StoreName { get; set; }

        public string FullName { get; set; }

        public string ContractNo { get; set; }

        public string RenterName { get; set; }

        public string Phone { get; set; }

        public decimal CharterMoney { get; set; }

        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public string ContractType { get; set; }

        public DateTime PCStartDate { get; set; }

        public DateTime PCEndDate { get; set; }

        public int DayCount { get; set; }

        public string IsPointShop { get; set; }

        public string SpecialRoomType { get; set; }
    }
}
