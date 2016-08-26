using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RentLoanService.Model
{
    public class RentLoanRisk
    {
        public int ID { get; set; }

        [Display(Name = "合同编号")]
        public string ContractNo { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "合同开始日期")]
        public DateTime StartDate { get; set; }

        [Display(Name = "合同结束日期")]
        public DateTime EndDate { get; set; }

        [Display(Name = "楼栋")]
        public string BuildingName { get; set; }

        [Display(Name = "房间")]
        public string FullName { get; set; }

        [Display(Name = "风控天数")]
        public int Risk { get; set; }

        [Display(Name = "租金贷合同状态")]
        public short Status { get; set; }

        [Display(Name = "门店ID")]
        public int StoreID { get; set; }

        [Display(Name = "门店名称")]
        public string StoreName { get; set; }

        [Display(Name = "上次还款覆盖结束日期")]
        public DateTime? PEndDate { get; set; }
    }
}
