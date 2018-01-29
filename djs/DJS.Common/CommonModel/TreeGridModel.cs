using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class TreeGridModel
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string text { get; set; }
        public bool isLeaf { get; set; }
        public bool expanded { get; set; }
        public bool loaded { get; set; }
        public string entityJson { get; set; }
    }
}
