using RentloanSettlementService.StoreSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.PaymentSpace
{
    public class DailyStorePayment
    {
        public int ID { get; set; }
        public int StoreID { get; set; }
        public virtual Store Store { get; set; }
        [Display(Name = "存款日")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }


        [Display(Name = "今日已存入金额(元)")]
        public decimal SavingCash { get; set; }
        [Display(Name = "剩余现金(元)")]
        public decimal RemainingCash { get; set; }

        [NotMapped]
        [Display(Name = "昨日剩余现金(元)")]
        public decimal LastRemaining { get; set; }
        [NotMapped]
        public CashSaving CashSaving { get; set; }

        [NotMapped]
        [Display(Name = "今日现金收入(元)")]
        public decimal TotalCash { get; set; }
    }
}
