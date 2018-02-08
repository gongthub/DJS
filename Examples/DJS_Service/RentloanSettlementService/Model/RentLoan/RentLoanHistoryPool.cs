using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterElectricService.Model.RentLoan
{
    public class RentLoanHistoryPool
    {
        public int ID { get; set; }
        [Display(Name = "租户姓名")]
        public string RenterName{get;set;}
        [Display(Name = "证件号")]
        public string CertificateNo{get;set;}
        [Display(Name = "系统时间")]
        public DateTime SystemDate{get;set;}
        [Display(Name = "审批额度")]
        public decimal ApprovalAmount{get;set;}
        [Display(Name = "总价格")]
        public decimal AggregatePrice{get;set;}
        [Display(Name = "期数")]
        public int Periods{get;set;}
        [Display(Name = "当期本金")]
        public decimal CurrentPrincipal{get;set;}
        [Display(Name = "还款金额")]
        public decimal RepaymentAmount{get;set;}
        [Display(Name = "还款时间")]
        public DateTime RepaymentTime{get;set;}
        [Display(Name = "创建时间")]
        public DateTime CreateDate{get;set;}
        [Display(Name = "创建用户")]
        public int CreateUserId{ get; set; }

        [Display(Name = "银行名称")]
        public String BankName { get; set; }

        [Display(Name = "门店ID")]
        public int? StoreID { get; set; }
    }
}
