using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Model
{
   public  class ModelDetail
    {

        public int ID { get; set; }
        public int ModelID { get; set; }
       
        public String Name { get; set; }
        public Int16 Status { get; set; }
        public Int16 Sort { get; set; }
        public Int16? IsShow { get; set; }

        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateUserID { get; set; }
    }
}
