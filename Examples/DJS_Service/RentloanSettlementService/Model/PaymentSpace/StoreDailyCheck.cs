using RentloanSettlementService.PaymentSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.PaymentSpace
{
    public class StoreDailyCheck
    {
        public int ID { get; set; }
        public int StoreID { get; set; }
        [Required]
        [Display(Name = "核对日期")]
        public DateTime CheckDate { get; set; }

        [Required]
        [Display(Name = "当日存款（元）")]
        public decimal SavingCash { get; set; }

        [Required]
        [Display(Name = "存款单据数（张）")]
        public short SavingCashCount { get; set; }

        [Required]
        [Display(Name = "剩余现金（元）")]
        public decimal LastCash { get; set; }

        [Required]
        [Display(Name = "当日收取现金（元）")]
        public decimal Cash { get; set; }

        [NotMapped]
        public List<StoreDailyCheckDetail> StoreDailyCheckDetailList { get; set; }

        [Required]
        [Display(Name = "实际操作日期")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Display(Name = "日审结束时间")]
        public DateTime CheckEndDate { get; set; }

        [Required]
        [Display(Name = "审核人")]
        public int UserID { get; set; }
    }

    public class StoreDailyCheckDetail
    {
        public int ID { get; set; }
        public int StoreDailyCheckID { get; set; }
        public StoreDailyCheck StoreDailyCheck { get; set; }
        public int PayMethodTypeID { get; set; }
        public virtual PayMethodType PayMethodType { get; set; }
        public decimal Amount { get; set; }
        public short Count { get; set; }
    }
}
