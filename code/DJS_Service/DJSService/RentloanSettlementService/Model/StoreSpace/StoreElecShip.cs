using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.StoreSpace
{
    public class StoreElecShip
    {
        public int ID { get; set; }
        public int StoreID { get; set; }

        public virtual Store Store { get; set; }
        public int TypeID { get; set; }//对应电表厂商类型

    }
}
