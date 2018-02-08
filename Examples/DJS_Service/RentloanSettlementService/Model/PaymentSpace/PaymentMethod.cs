using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.PaymentSpace
{
    public class PaymentMethod
    {
        public int ID { get;set;}

        [Display(Name = "支付日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? Date { get;set;}

        [Display(Name = "支付类型")]
        public byte PayType	{ get;set;}

        [Display(Name = "创建人ID")]
        public int? CreateUserID	{ get;set;}

        [Display(Name = "创建日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? CreateDate	{ get;set;}

        [Display(Name = "金额")]
        public decimal? TotalAmount	{ get;set;}

        [Display(Name = "银行卡ID")]
        public int? BankCardID { get; set; }

        [Display(Name = "银行卡")]
        public virtual BankCard BankCard { get; set; }

        [Display(Name = "流水号")]
        public string PaySerialNo { get; set; }

        [Display(Name = "支付状态")]
        public int Status { get; set; }

        [NotMapped]
        public int ContractID { get; set; }

    }
}
