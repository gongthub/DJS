using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.PaymentSpace
{
    public class PaymentDetails
    {
        public int ID { get; set; }

        [Display(Name = "金额")]
        public decimal Amount { get; set; }

        [Display(Name = "支付类型")]
        public byte Type { get; set; }

        [Display(Name = "流水号")]
        public string SerialNo { get; set; }

        [Display(Name = "支付ID")]
        public int PaymentMethodID {  get;set; }

        [Display(Name = "支付方式")]
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
