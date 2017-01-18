using RentloanSettlementService.PaymentSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.PaymentSpace
{
    public class DailyStorePaymentDetail
    {
        public int ID { get; set; }
        public int PayMethodTypeID { get; set; }
        public decimal Amount { get; set; }

        public int DailyStorePaymentID { get; set; }

        public virtual DailyStorePayment DailyStorePayment { get; set; }

        public virtual PayMethodType PayMethodType { get; set; }
    }
}
