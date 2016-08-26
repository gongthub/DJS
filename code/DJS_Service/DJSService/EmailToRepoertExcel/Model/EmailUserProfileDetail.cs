using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Model
{
    public class EmailUserProfileDetail
    {
        public int ID { get; set; }

        public int CategoryID { get; set; }

        public int UserID { get; set; }

        public int Status { get; set; }

        public int StoreID { get; set; }

        public DateTime CreateDate { get; set; }

        public int Type  { get; set; }

       //用户
        //public UserProfile UserProfile { get; set; }
    }
}
