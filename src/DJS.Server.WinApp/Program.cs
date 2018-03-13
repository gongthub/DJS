using DJS.Core.CPlatform;
using DJS.Core.DotNetty;
using DJS.Core.Server;
using System;

namespace DJS.Server.WinApp
{
    class Program
    {
        static void Main(string[] args)
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
            Console.WriteLine("服务启动成功！");
            Console.ReadKey();
        }
    }
}
