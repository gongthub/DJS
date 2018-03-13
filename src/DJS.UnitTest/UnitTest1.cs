using DJS.Core.CPlatform;
using DJS.Core.CPlatform.Address;
using DJS.Core.CPlatform.Transport.Codec.Implementation;
using DJS.Core.DotNetty;
using DJS.Core.Scheduler;
using DJS.Core.Scheduler.Utilities;
using DJS.Core.Server;
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
        public void SchedulerTest()
        {
            //var builder = new ConfigurationBuilder()
            //.AddSchedulerFile("Configs/schedulerSettings.json", optional: false)
            //.AddServerInfo("127.0.0.1",11120);

            //ISchedulerProvider _schedulerBuilder;
            //_schedulerBuilder = SchedulerContainer.GetInstances<ISchedulerProvider>("Polling");
            //_schedulerBuilder
            //    .UseDotNettyTransport()
            //    .BulidOnStartRunJob(OnStartRunJob)
            //    .BulidOnCompleteRunJob(OnCompleteRunJob)
            //    .Start();

            var host = new SchedulerBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddServices(option =>
                    {
                        option.UseDotNettyTransport()
                        .UsePolling();
                    });
                })
                .AddServerInfo("127.0.0.1", 11120)
               .Build();
            //host.BulidOnStartRunJob(OnStartRunJob);
            //host.BulidOnCompleteRunJob(OnCompleteRunJob);

            host.Start();
            Console.ReadKey();
        }


        private void OnStartRunJob(object sender, SchedulerEventArgs e)
        {
            Debug.WriteLine(e.job.Name + " ��ʼ ִ��ʱ�䣺" + DateTime.Now + " �´ο�ʼʱ�䣺" + e.job.NextTime);
        }
        private void OnCompleteRunJob(object sender, SchedulerEventArgs e)
        {
            Debug.WriteLine(e.job.Name + " ��� ִ��ʱ�䣺" + DateTime.Now + " �´ο�ʼʱ�䣺" + e.job.NextTime);
        }

        [TestMethod]
        public void ServerTest()
        {
            var host = new ServerBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddServices(option =>
                    {
                        option.AddServerServices()
                        .UseDotNettyTransport();
                    });
                })
                .AddServerInfo("127.0.0.1", 11120)
               .Build();

            host.Start();
            Console.ReadKey();
        }

        [TestMethod]
        public void DotNettyTest()
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //var host = new ServiceHostBuilder()
            //    .RegisterServices(builder =>
            //    {
            //        builder.UseDotNettyTransport();
            //        builder.AddMicroService(option =>
            //        {
            //            option.AddClientIntercepted(typeof(CacheProviderInterceptor));
            //        });
            //    })
            //    .UseServer(options => {
            //        options.Ip = "127.0.0.1";
            //        options.Port = 98;
            //        options.ExecutionTimeoutInMilliseconds = 30000;
            //        options.MaxConcurrentRequests = 200;
            //    })
            //    .UseStartup<Startup>()
            //    .Build();

            //using (host.Run())
            //{
            //    Console.WriteLine($"����������ɹ���{DateTime.Now}��");
            //}
            IpAddressModel IpModel = new IpAddressModel("127.0.0.1", 11112);
            DotNettyServerMessageListener lis = new DotNettyServerMessageListener(
                 new JsonTransportMessageCodecFactory());
            lis.StartAsync(IpModel.CreateEndPoint());


            Console.ReadKey();
        }

    }


    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfigurationBuilder config)
        {
        }
    }
}
