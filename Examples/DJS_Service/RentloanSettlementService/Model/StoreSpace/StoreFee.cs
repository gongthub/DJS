using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RentloanSettlementService.StoreSpace
{
    public class StoreFee
    {
        public int ID { get; set; }
        public int StoreID { get; set; }
        public virtual Store Store { get; set; }

        [Display(Name = "公摊水电费")]
        public decimal PublicWE { get; set; }

        [Display(Name = "公共卫生费")]
        public decimal PublicClean { get; set; }

        [Display(Name = "预定费")]
        public decimal Deposit { get; set; }

        [Display(Name = "电视费")]
        public decimal? CATV { get; set; }

        [Display(Name = "宽带/电视费")]
        public decimal Network { get; set; }

        [Display(Name = "电费")]
        public decimal Electric { get; set; }

        [Display(Name = "冷水费")]
        public decimal ColdWater { get; set; }

        [Display(Name = "热水费")]
        public decimal HotWater { get; set; }

        [Display(Name = "转房费")]
        public decimal ChangeRoom { get; set; }

        [Display(Name = "签约费")]
        public decimal Sign { get; set; }

        [Display(Name = "电视费用成本")]
        public decimal? CATVCost { get; set; }

        [Display(Name = "网费成本")]
        public decimal NetworkCost { get; set; }

        [Display(Name = "电费成本")]
        public decimal ElectricCost { get; set; }

        [Display(Name = "冷水费成本")]
        public decimal ColdWaterCost { get; set; }

        [Display(Name = "热水费成本")]
        public decimal HotWaterCost { get; set; }

        [Display(Name = "宽带/电视费包含在房租内")]

        public decimal NetworkInCharterMoney { get; set; }


        [Display(Name = "状态")]

        public short Status { get; set; }

        [NotMapped]
        [Display(Name = "宽带/电视费包含在房租内")]
        public bool isNetworkInCharterMoney { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "修改人")]
        public int? ModifiedUser { get; set; }

        [Display(Name = "水电预缴充值阀值")]
        public decimal WaterElectChargeWarn { get; set; }
        
        [Display(Name = "水电余额阀值")]
        public decimal WaterElectBalanceWarn { get; set; }
    }
}
