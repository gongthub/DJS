using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMPProjectSummary.Model
{
    public class ProjectNode
    {
        public int ID { get; set; }
        public int ModelNodeID { get; set; }
       
        public int ProjectDetailID { get; set; }
        public virtual ProjectDetail ProjectDetail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RealStartDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public Int16 Status { get; set; }
        public string DelayReasonID { get; set; }

      
        //public virtual DelayReason DelayReason { get; set; }
        public String DelayContent { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedUserID { get; set; }
       
    }
}
