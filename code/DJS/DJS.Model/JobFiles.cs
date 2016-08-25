using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class JobFiles
    {
        public Guid ID { set; get; }
        public Guid JobID { set; get; }
        public string Name { set; get; }
        public string JobName { set; get; }
        public string Src { set; get; }
    }
}
