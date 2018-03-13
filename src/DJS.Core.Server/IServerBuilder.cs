using Autofac;
using DJS.Core.CPlatform.Server;
using System;

namespace DJS.Core.Server
{
    public interface IServerBuilder
    {
        IServerProvider Build();

        IServerBuilder RegisterServices(Action<ContainerBuilder> builder);

        IServerBuilder MapServices(Action<IContainer> mapper);
    }
}
