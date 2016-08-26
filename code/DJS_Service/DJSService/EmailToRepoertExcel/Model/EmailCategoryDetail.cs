using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Model
{
    public class EmailCategoryDetail
    {
        public int ID { get; set; }

        public string ModelID { get; set; }

        public int CategoryID { get; set; }

        //[ForeignKey("ModelID")]
        //public virtual ICollection<EmailModel> EmailModel { get; set; }
    }
}
