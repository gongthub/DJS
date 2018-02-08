using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public class ModuleEntity : BaseEntity<ModuleEntity>
    {
        public string ParentId { get; set; }
        public int? Layers { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string UrlAddress { get; set; }
        public string Target { get; set; }
        public bool? IsMenu { get; set; }
        public bool? IsExpand { get; set; }
        public bool? IsPublic { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public int? SortCode { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
    }
}
