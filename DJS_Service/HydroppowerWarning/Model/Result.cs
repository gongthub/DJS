using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydroppowerWarning.Model
{
    public class Result
    {
        public Result()
        {
            desc = "";
        }

        public bool success { get; set; }

        public string desc { get; set; }

        public object data { get; set; }
    }
}
