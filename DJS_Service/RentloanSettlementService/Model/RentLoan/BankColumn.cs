using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterElectricService.Model.RentLoan
{
    public class BankColumn
    {
        public int ID { get; set; }

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

        [Display(Name = "其他")]
        public string OtherDesc { get; set; }

        [Display(Name = "状态")]
        public short Status { get; set; }

        [Display(Name = "账单日期")]
        public int? StatementDate { get; set; }

        [Display(Name = "银行名称")]
        public String BankName { get; set; }
    }
}
