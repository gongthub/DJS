using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Model
{
    public class EmailModel
    {
        public int ID { get; set; }

        public string ModelID { get; set; }

        public int Type { get; set; }

        public string Contents { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
