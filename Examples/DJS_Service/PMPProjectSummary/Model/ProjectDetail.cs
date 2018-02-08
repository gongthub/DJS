using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Model
{
    public class ProjectDetail
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
       
        public int ModelDetailID { get; set; }
        public virtual ModelDetail ModelDetail { get; set; }
        public Int16 Status { get; set; }
       
    }
}
