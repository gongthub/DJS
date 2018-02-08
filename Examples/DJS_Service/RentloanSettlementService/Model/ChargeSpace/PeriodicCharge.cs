using RentloanSettlementService.PaymentSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentloanSettlementService.StoreSpace;
using System.ComponentModel.DataAnnotations.Schema;
using RentloanSettlementService.ContractSpace;

namespace OpsModel.Charge
{
    /// <summary>
    /// 阶段性收费
    /// </summary>
    public class PeriodicCharge
    {
        public int ID { get; set; }

        [Display(Name = "合同ID")]
        public int ContractID { get; set; }

        [Display(Name = "合同")]
        public virtual Contract Contract { get; set; }

        [Display(Name = "开始日期")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "结束日期")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "金额")]
        public decimal Amount { get; set; }

        [Display(Name = "科目")]
        public short FeeType { get; set; }

        [Display(Name = "优惠代码")]
        public int? CouponID { get; set; }

        public virtual Coupon Coupon { get; set; }

        [Display(Name = "支付方法ID")]
        public int? PaymentMethodID { get; set; }

        [Display(Name = "支付方法")]
        public virtual PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "备注")]
        public string Comment { get; set; }

        public DateTime? PayDate { get; set; }

        [NotMapped]
        public bool? IsSettlement { get; set; }

        private int _Status = (int)1;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        [Display(Name = "应收金额")]
        public decimal ReceivableAmount { get; set; }
    }
}
