using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpsModel.Charge
{
    /// <summary>
    /// 阶段性收费明细
    /// </summary>
    public class PeriodicChargeDetail
    {
        public int ID	{ get;set;}

        [Display(Name = "收费ID")]
        public int PeriodicChargeID	{ get;set;}

        [Display(Name = "收费科目")]
        public virtual PeriodicCharge PeriodicCharge { get; set; }

        [Display(Name = "名称")]
        public string Name	{ get;set;}

        [Display(Name = "单价")]
        public decimal UnitPrice	{ get;set;}

        [Display(Name = "数量")]
        public decimal Quantity	{ get;set;}

        [Display(Name = "单位")]
        public string Unit	{ get;set;}

        [Display(Name = "类型")]
        public Byte Type { get; set; }
        [Display(Name = "产品编码")]
        public string ProductCode { get; set; }

        //产品类型（区分是否是优惠）
        private short _productType = (short)1;
        public short ProductType
        {
            get { return _productType; }
            set { _productType = value; }
        }
    }
}
