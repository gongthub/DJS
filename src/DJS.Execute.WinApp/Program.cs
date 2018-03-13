using DJS.Core.CPlatform;
using DJS.Core.DotNetty;
using DJS.Core.Execute;
using System;

namespace DJS.Execute.WinApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ExecuteBuilder()
                .RegisterServices(builder =>
                {
                    builder.AddServices(option =>
                    {
                        option.AddExecuteServices().
                        UseDotNettyTransport();
                    });
                })
                .AddServerInfo("127.0.0.1", 11120)
               .Build();
            host.Start();
            Console.ReadKey();
        }
    }
}
