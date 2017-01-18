using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsModel.Charge
{
    /// <summary>
    /// 收费明细
    /// </summary>
    public class ChargeDetail
    {
        public int ID { get; set; }

        [Display(Name = "收费ID")]
        public int ChargeID	{ get; set; }

        [Display(Name = "收费")]
        public virtual Charge Charge { get; set; } 

        [Display(Name = "名称")]
        public string Name	{ get; set; }

        [Display(Name = "单价")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "数量")]
        public decimal Quantity	{ get; set; }

        [Display(Name = "单位")]
        public string Unit	{ get; set; }

        [Display(Name = "类型")]
        public Byte Type { get; set; }

        [NotMapped]
        public String Remark { get; set; }

        [Display(Name = "产品编码")]
        public string ProductCode { get; set; }

        //产品类型（区分是否是优惠 默认值为非优惠）
        private short _productType =1;
        public short ProductType
        {
            get { return _productType; }
            set { _productType = value; }
        }

    }
}
