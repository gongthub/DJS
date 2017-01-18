using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.PaymentSpace
{
    public class BankCard
    {
        public  int ID { get;set;}

        [Display(Name = "持卡人姓名")]
        public string Name	{ get;set;}

        [Display(Name = "银行卡号")]
        public string CardNo { get;set;}

        [Display(Name = "银行")]
        public string BankInfo	{ get;set;}

        public string SSN { get; set; }

        [Display(Name = "所属支行")]
        public string BelongsBranch { get; set; }

        public int? Status { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(CardNo) || !string.IsNullOrEmpty(BankInfo) || !string.IsNullOrEmpty(BelongsBranch));
        }
    }
}
