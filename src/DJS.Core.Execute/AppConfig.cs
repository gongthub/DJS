using Autofac;
using DJS.Core.Common.Net;
using DJS.Core.CPlatform.Address;
using DJS.Core.CPlatform.Execute.Models;
using DJS.Core.CPlatform.Scheduler.Models;
using System.Collections.Generic;

namespace DJS.Core.Execute
{
    public class AppConfig
    {
        internal static IpAddressModel SERVERINFO { get; set; }
        internal static ContainerBuilder SERVICES { get; set; }
        internal static IContainer ICONTATINER { get; set; }

        private static List<ExecuteJobModel> JOBMODELS { get; set; }

        static AppConfig()
        {
            SERVERINFO = new IpAddressModel();
            SERVICES = new ContainerBuilder();
            JOBMODELS = new List<ExecuteJobModel>();
        }

        public static void SetJob(ExecuteJobModel job)
        {
            ExecuteJobModel model = JOBMODELS.Find(m => m.Id == job.Id);
            if (model == null)
            {
                HttpHelp.FileDownSave(job.AssPath, "Stores/");

                
                JOBMODELS.Add(job);
            }
        }
        public static List<ExecuteJobModel> GetJobs()
        {
            return JOBMODELS;
        }
    }
}
