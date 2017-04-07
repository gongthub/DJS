using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanFinancialService.Model
{
    public class ContractOps
    {
        public int ID { get; set; }

        [Display(Name = "合同ID")]
        public int ContractID { get; set; }

        public virtual Contract Contract { get; set; }

        public byte OperationType { get; set; }

        public string Comment { get; set; }

        [Display(Name = "创建人ID")]
        public int CreateUserID { get; set; }

        [Display(Name = "创建日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CreateDate { get; set; }

        public byte? CheckOutType { get; set; }
    }
}
