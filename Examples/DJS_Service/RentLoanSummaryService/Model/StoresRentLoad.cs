using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Model
{
    public class StoresRentLoad
    {
        [Display(Name = "社区编号")]
        public int StoreId { get; set; }
        [Display(Name = "社区名称")]
        public string StoreName { get; set; }
        [Display(Name = "红色预警:负数日期")]
        public int RedWarnings { get; set; }
        [Display(Name = "橙色预警:<=15天")]
        public int OrangeWarnings { get; set; }
        [Display(Name = "黄色预警:16-30天")]
        public int YellowWarnings { get; set; }
        [Display(Name = "蓝色预警:31-45天")]
        public int BlueWarnings { get; set; }
        [Display(Name = "常规预警:大于45天")]
        public int NormalWarnings { get; set; }
        [Display(Name = "备注")]
        public string Description { get; set; }
    }
}
