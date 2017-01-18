using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterElectricService.Model.ItemSpace
{
    public class UserProfile
    {
            [Key]
        	public int UserId{get;set;}
	        public string UserName{get;set;}
            public string Email { get; set; }
            public string Phone { get; set; }
    }
}
