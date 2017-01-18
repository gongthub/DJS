using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Model
{
    public class Stores
    {
        [Display(Name = "社区编号")]
        public int Id { get; set; }
        [Display(Name = "社区名称")]
        public string Name { get; set; }
    }
}
