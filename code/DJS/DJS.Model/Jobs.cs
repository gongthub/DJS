using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class Jobs
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string GroupName { set; get; }
        public string TriggerName { set; get; }
        public string TriggerGroup { set; get; }

        public int State { set; get; }
    }
}
