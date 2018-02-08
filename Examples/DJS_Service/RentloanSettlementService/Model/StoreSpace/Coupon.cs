using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentloanSettlementService.StoreSpace
{
    public class Coupon
    {
        public int ID { get; set; }
        [Display(Name = "受惠人")]
        public string UserName { get; set; }
        public int StoreID { get; set; }
        [Display(Name = "店名")]
        public virtual Store Store { get; set; }
        [Display(Name = "优惠代码")]
        public string CouponNo { get; set; }
        [Display(Name = "优惠描述")]
        public string Comment { get; set; }
        public short CouponType { get; set; }
        [Display(Name = "优惠内容")]
        public short CouponOn { get; set; }
        public short DiscountType { get; set; }
        [Display(Name = "优惠尺度")]
        public decimal Number { get; set; }
        [Display(Name = "起始时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }
        [Display(Name = "结束时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }
        public short Status { get; set; }
        [Display(Name = "优惠名称")]
        public string CouponName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUser { get; set; }


        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public decimal GetDiscountAmount()
        {
            throw new NotImplementedException();
        }
    }
}
