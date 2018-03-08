using Autofac;
using DJS.Core.CPlatform.Communication;
using System;

namespace DJS.Core.Communication
{
    public interface ICommunicationBuilder
    {
        ICommunicationProvider Build();

        ICommunicationBuilder RegisterServices(Action<ContainerBuilder> builder);

        ICommunicationBuilder MapServices(Action<IContainer> mapper);
    }
}
