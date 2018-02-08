using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterElectricService.Model.RentLoan
{
    public class RentLoanAudit
    {
        public int ID { get; set; }

        [Display(Name = "租户编号")]
        public int RenterID { get; set; }

        [Display(Name = "商户名称")]
        public string MerchantName { get; set; }

        [Display(Name = "专项消费名称")]
        public string ConsumptionName { get; set; }

        [Display(Name = "专项消费地址")]
        public string ConsumptionAddress { get; set; }

        [Display(Name = "总价格")]
        public decimal? AggregatePrice { get; set; }

        [Display(Name = "首付额度")]
        public decimal? DownPayment { get; set; }

        [Display(Name = "申请额度")]
        public decimal? ApplyQuota { get; set; }

        [Display(Name = "分期数")]
        public int? Periods { get; set; }

        [Display(Name = "手续费收取方式")]
        public byte? PoundageCollectType { get; set; }

        [Display(Name = "有无资产证明文件")]
        public byte? IsNoAssetFile { get; set; }

        [Display(Name = "卡片简称")]
        public string CardName { get; set; }

        [Display(Name = "卡片代码")]
        public string CardNo { get; set; }

        [Display(Name = "卡片寄送地址")]
        public byte? CardSendAddress { get; set; }

        [Display(Name = "账单邮箱")]
        public string MailTheBill { get; set; }

        [Display(Name = "分期类型")]
        public string StagesAndType { get; set; }

        [Display(Name = "分行机构码")]
        public string BankCode { get; set; }

        [Display(Name = "推荐人代码")]
        public string RecomPersonCode { get; set; }

        [Display(Name = "城市代码")]
        public string CityCode { get; set; }

        [Display(Name = "营销人员单位")]
        public string SalesManOrDp { get; set; }

        [Display(Name = "活动代码")]
        public string ActiveCode { get; set; }

        [Display(Name = "其他信息")]
        public string OtherDesc { get; set; }

        [Display(Name = "状态")]
        public short Status { get; set; }

        [Display(Name = "已还期数")]
        public int? AlreadyAlsoPeriod { get; set; }

        [Display(Name = "放款日期")]
        public DateTime? DateOfLoan { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "创建人ID")]
        public int? CreateUserId { get; set; }

        [Display(Name = "月付费金额")]
        public decimal MonthlyTotalAmount { get; set; }

        [Display(Name = "总金额")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "合同")]
        public virtual RentloanSettlementService.StoreSpace.Renter Renter { get; set; }

        public int BankColumnID { get; set; }

        [Display(Name="银行")]
        public virtual BankColumn BankColumn { get; set; }

        public virtual List<WaterElectricService.Model.RentLoan.RentLoanAuditLog> RentLoanAuditLogs { get; set; }

    }
}
