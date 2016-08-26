using RentloanSettlementService.PaymentSpace;
using RentloanSettlementService.StoreSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.Reservation
{
    public class ReservationCharge
    {
        public int ID{get;set;}
        public int ReservationID{get;set;}
        public virtual Reservation Reservation{ get; set; }
        public decimal Amount{get;set;}
        public int? CouponID{get;set;}
        public virtual Coupon Coupon { get; set; }
        public int? PaymentMethodID{get;set;}
        public virtual PaymentMethod PaymentMethod { get; set; }

        private int _Status = (int)Common.CommonEnum.RecordStatus.正常;
        
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

         [NotMapped]
        public String DisplayName { get; set; }
    }
}
