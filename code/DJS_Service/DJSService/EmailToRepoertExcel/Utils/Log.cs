using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailToRepoertExcel.Utils
{
    public  class Log
    {
        public static NLog.Logger Instance()
        {
            return NLog.LogManager.GetCurrentClassLogger();
        }

    }
}
