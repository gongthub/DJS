using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RentWarningSMS.Model
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
