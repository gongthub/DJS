using RentloanSettlementService.StoreSpace;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterElectricService.Model.ItemSpace
{
    public class Voucher
    {
            public int ID { get; set; }

            [Display(Name = "门店")]
            public int? StoreID { get; set; }

            public virtual Store Store { get; set; }

            [Display(Name = "现金抵用券使用者")]
            public string UserName { get; set; }

            [Display(Name = "现金抵用券名称")]
            [Required]
            public string VoucherName { get; set; }

            [Display(Name = "现金抵用券代码")]
            [Required]
            public string VoucherNo { get; set; }

            [Display(Name = "现金抵用券描述")]
            [Required]
            public string Comment { get; set; }

            [Display(Name = "金额")]
            public decimal Amount { get; set; }

            [Display(Name = "起始时间")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime StartDate { get; set; }

            [Display(Name = "结束时间")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime EndDate { get; set; }

            public short Status { get; set; }

            public DateTime? ModifiedDate { get; set; }

            public int? ModifiedUser { get; set; }

            //来源
            public int? Source { get; set; }

            //折扣类型
            public short? DiscountType { get; set; }

            //最低使用金额
            public decimal? Limit { get; set; }

            //覆盖月份数
            public int? Month { get; set; }

            //优惠券类别
            public short? VoucherType { get; set; }


            [Display(Name = "申请日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime? CreateDate { get; set; }

       


    }
}
