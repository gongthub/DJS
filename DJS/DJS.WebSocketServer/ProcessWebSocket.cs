using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.WebSocketServer
{
    public class ProcessWebSocket
    {
        private static int RUNJOBNUMS = 0;
        public string DoProcess(string msg)
        {
            string strObj = string.Empty;
            switch (msg)
            { 
                case"RUNJOBNUMS":
                    strObj = RunJobs();
                    break;
            }
            return strObj;
        }

        private string RunJobs()
        {
            string strret = string.Empty;
            IList<IJobExecutionContext> models = Common.QuartzHelp.quartzHelp.GetCurrentlyExecutingJobs();
            if (models != null)
            {
                int runJobNum = models.Count;
                if (runJobNum != RUNJOBNUMS)
                {
                    strret = runJobNum.ToString();
                    RUNJOBNUMS = runJobNum;
                }
            }
            return strret;
        }

    }
}
