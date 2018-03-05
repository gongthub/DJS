using Autofac;
using DJS.Core.Scheduler;
using DJS.Core.Scheduler.Configurations;
using DJS.Core.Scheduler.Utilities;
using DJS.Core.System.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace DJS.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddSchedulerFile("Configs/schedulerSettings.json", optional: false);
            
            ISchedulerProvider _schedulerBuilder;
            _schedulerBuilder = SchedulerContainer.GetInstances<ISchedulerProvider>("Polling");
            _schedulerBuilder
                .BulidOnStartRunJob(OnStartRunJob)
                .BulidOnCompleteRunJob(OnCompleteRunJob)
                .Start();
        }


        private void OnStartRunJob(object sender, SchedulerEventArgs e)
        {
            Debug.WriteLine(e.job.Name + " 开始");
        }
        private void OnCompleteRunJob(object sender, SchedulerEventArgs e)
        {
            Debug.WriteLine(e.job.Name + " 完成");
        }
    }
}
