using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class Contract
    {
        public int ID { get; set; }

        [Display(Name = "房间ID")]
        public int RoomID { get; set; }

        [Display(Name = "租客ID")]
        public int RenterID { get; set; }

        [Display(Name = "合租人ID")]
        public int? OtherRenterID { get; set; }

        [Display(Name = "合同开始日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "合同结束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "实际结束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? RealEndDate { get; set; }

        [Display(Name = "紧急联系人")]
        public string ECName { get; set; }

        [Display(Name = "紧急联系人电话")]
        public string ECPhone { get; set; }

        [Display(Name = "和租户关系")]
        public string ECRelationship { get; set; }

        [Display(Name = "银行卡ID")]
        public int? BankCardID { get; set; }

        [Display(Name = "类型")]
        public short Action { get; set; }

        [Display(Name = "上一份合同的ID")]
        public int? PreContractID { get; set; }
        public virtual Contract PreContract { get; set; }

        [Display(Name = "合同号")]
        public string ContractNo { get; set; }

        [Display(Name = "预定ID")]
        public int? ReservationID { get; set; }

        [Display(Name = "房租定价")]
        [NotMapped]
        public decimal? CharterMoney { get; set; }

        [Display(Name = "合同状态")]
        public short Status { get; set; }

        [Display(Name = "是否有费用")]
        [NotMapped]
        public bool HaveFee { get; set; }

        [Display(Name = "是否有需要收费的")]
        [NotMapped]
        public bool NeedPay { get; set; }

        [Display(Name = "客户渠道ID")]
        public int? CustomerChannelID { get; set; }

        [Display(Name = "客户渠道备注")]
        public String ChannelComment { get; set; }


        [Display(Name = "合同周期")]
        public int? ContractMonth { get; set; }

        [Display(Name = "合同类型")]
        public int? Type { get; set; }

        [Display(Name = "短租标识")]
        public int? ShortFlag { get; set; }

        [Display(Name = "客户类型")]
        public int? ContractFlag { get; set; }

        [Display(Name = "首次付费月数")]
        public int? FirstPaymentMonth { get; set; }
        [Display(Name = "销售员二维码")]
        public string DistributionCode { get; set; }
        //public virtual List<RentLoan.RentLoanAudit> RentLoanAudits { get; set; }
        /// <summary>
        /// 是否有异议
        /// </summary>
        public bool? Objection { get; set; }

        /// <summary>
        /// 预付金额
        /// </summary>
        [NotMapped]
        public decimal PrePay { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        [NotMapped]
        public decimal Deposit { get; set; }
        /// <summary>
        /// 日租金
        /// </summary>
        [NotMapped]
        public decimal DayRent { get; set; }

        /// <summary>
        /// 追加
        /// </summary>
        [NotMapped]
        public float AddFeeList { get; set; }

        [NotMapped]
        public int SaveFlag { get; set; }

        [NotMapped]
        public bool WaterCardDeposit { get; set; }

        [NotMapped]
        public bool ElectricCardDeposit { get; set; }

        [NotMapped]
        public bool AccessCardDeposit { get; set; }

        public int? ReturnMoneyType { get; set; }

        public int? ReturnLeaseSource { get; set; }

        [NotMapped]
        public string StartTimes { get; set; }
        [NotMapped]
        public string EndTimes { get; set; }
    }
}
