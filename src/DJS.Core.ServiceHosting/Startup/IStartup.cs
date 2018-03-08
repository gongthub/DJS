using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.ServiceHosting.Startup
{
   public  interface IStartup
    {
        IContainer ConfigureServices(ContainerBuilder services);

        void Configure(IContainer app);
    }
}
