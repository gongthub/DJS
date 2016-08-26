using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterElectricService.Model.RentLoan
{
    public class RentLoanAuditLog
    {
        public int ID { get; set; }

        [Display(Name = "租金贷审核流程")]
        public int RentLoanAuditId { get; set; }

        [Display(Name = "日期")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "操作者")]
        public int ModifiedUserId { get; set; }

        [Display(Name = "审核内容")]
        public string Desc { get; set; }

        [Display(Name = "状态")]
        public short Status { get; set; }

        public RentLoanAudit RentLoanAudit { get; set; }
    }
}
