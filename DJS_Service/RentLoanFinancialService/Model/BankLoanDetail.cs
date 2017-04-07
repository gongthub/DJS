using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class BankLoanDetail
    {
        public int ID { get; set; }

        [Display(Name = "商户名称")]
        public string MerchantName { get; set; }

        [Display(Name = "商户代码")]
        public string MerchantNo { get; set; }

        [Display(Name = "交易日期")]
        public DateTime TradeDate { get; set; }

        [Display(Name = "结算日期")]
        public DateTime SettlementDate { get; set; }

        [Display(Name = "租户姓名")]
        public string RenterName { get; set; }

        [Display(Name = "租户身份证号")]
        public string CertificateNo { get; set; }

        [Display(Name = "卡片后四位")]
        public string CardNo { get; set; }

        [Display(Name = "期数")]
        public int Periods { get; set; }

        [Display(Name = "交易金额")]
        public decimal TradeAmount { get; set; }

        [Display(Name = "结算金额")]
        public decimal SettlementAmount { get; set; }

        [Display(Name = "手续费")]
        public decimal CounterFee { get; set; }

        [Display(Name = "租金贷流程")]
        public int? RentLoanAuditID { get; set; }

        public virtual RentLoanAudit rentLoanAudit { get; set; }

        [Display(Name = "银行名称")]
        public String BankName { get; set; }
        //账单日（互联网金融）
        public int? StatementDay { get; set; }

        [Display(Name = "门店ID")]
        public int? StoreID { get; set; }

        //[Display(Name = "门店")]
        //public virtual OpsModel.Store.Store Store { get; set; }
    }
}
