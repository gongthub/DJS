using RentloanSettlementService.ContractSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterElectricService.Model.RentLoan
{
    public class RentLoanSettlementLog
    {
        public int ID{get;set;}

        [Display(Name = "合同编号")]
        public int ContractID{get;set;}
        [Display(Name = "历史池编号")]

        public int RentLoanHistoryPoolID { get; set; }                    
        [Display(Name = "期数")]
        public int Periods{get;set;}
        [Display(Name = "还款金额")]
        public decimal RepaymentAmount{get;set;}
        [Display(Name = "还款时间")]
        public DateTime RepaymentTime { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreateDate{get;set;}
        [Display(Name = "创建人")]
        public int CreateUserId { get; set; }
        [Display(Name = "历史池")]
        public RentLoanHistoryPool RentLoanHistoryPool { get; set; }

        public Contract Contract { get; set; }
    }
}
