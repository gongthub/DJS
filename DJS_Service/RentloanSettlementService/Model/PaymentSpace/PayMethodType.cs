using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.PaymentSpace
{
    public class PayMethodType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public int Source { get; set; }
        public int HasSerialNo { get; set; }
        public int IsReturn { get; set; }
        public int IsCheckNo { get; set; }
        public int OrderNo { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int IsDC { get; set; }
        public string DCAmountDisplayName { get; set; }
        public string DCCountDisplayName { get; set; }
    }
}
