using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RentloanSettlementService.ChargeSpace
{
    public class ChargeAccountType
    {
        public int ID { get; set; }

        [Display(Name = "收费科目类型")]       
        public short FeeType { get;set;}

        [Display(Name = "收费科目名称")]       
        public String Name { get;set;}

        [Display(Name = "收费科目号")]       
        public String AccountNo { get;set;}

        [Display(Name = "收费科目类别")]       
        public short ChargeType { get;set;}//1 周期费用 2一次费用 3 门店收费 4 预定费用

        [Display(Name = "是否是增值服务或其它费用")]  
        public bool IsOther { get;set;}

        [Display(Name = "店长是否有权限")]  
        public bool StoreManagerHasRight { get;set;}

         [Display(Name = "状态")]       
        public short Status { get;set;}

        [Display(Name = "订单科目")]
         public string OrderCategory { get; set; }
    }
}
