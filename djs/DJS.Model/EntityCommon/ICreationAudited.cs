using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Model
{
    public interface ICreationAudited
    {
        string ID { get; set; }
        string CreatorUserId { get; set; }
        DateTime? CreatorTime { get; set; }
    }
}
