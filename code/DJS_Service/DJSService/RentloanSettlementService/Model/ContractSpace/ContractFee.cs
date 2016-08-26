using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.ContractSpace
{
    public class ContractFee
    {
        public int ID { get; set; }

        [Display(Name = "合同ID")]
        public int ContractID { get; set; }
        public virtual Contract Contract { get; set; }

        [Display(Name = "合同房价")]
        public decimal CharterMoney { get; set; }

        [Display(Name = "公摊水电费")]
        public decimal PublicWE { get; set; }

        [Display(Name = "公共卫生费")]
        public decimal PublicClean { get; set; }

        [Display(Name = "TV费用")]
        public decimal CATV { get; set; }
    
        [Display(Name = "宽带费用")]
        public decimal Network { get; set; }   
    
        [Display(Name = "电费费用")]
        public decimal Electric { get; set; }

        [Display(Name = "冷水费用")]
        public decimal ColdWater { get; set; }
    
        [Display(Name = "热水费用")]
        public decimal HotWater { get; set; }

        [Display(Name = "押金")]
        public decimal Deposit { get; set; }

        [Display(Name = "水卡押金")]
        public decimal WaterCardCost { get; set; }

        [Display(Name = "电卡押金")]
        public decimal ElectricCardCost { get; set; }

        [Display(Name = "门禁押金")]
        public decimal EntranceCost { get; set; }

        [Display(Name = "房价优惠ID")]
        public int? RentCouponID { get; set; }
        public virtual RentloanSettlementService.StoreSpace.Coupon RentCoupon { get; set; }
    }
}
