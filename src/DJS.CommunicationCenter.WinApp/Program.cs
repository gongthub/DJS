using DJS.Core.Communication;
using DJS.Core.CPlatform;
using DJS.Core.DotNetty;
using System;

namespace DJS.CommunicationCenter.WinApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new CommunicationBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddServices(option =>
                    {
                        option.AddCommunicationServices()
                        .UseDotNettyTransport();
                    });
                })
                .AddServerInfo("127.0.0.1", 11120)
               .Build();

            host.Start();
            Console.WriteLine("服务启动成功！");
            Console.ReadKey();
        }
    }
}
