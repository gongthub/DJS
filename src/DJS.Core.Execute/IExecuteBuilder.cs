using Autofac;
using DJS.Core.CPlatform.Execute;
using System;

namespace DJS.Core.Execute
{
    public interface IExecuteBuilder
    {
        IExecuteProvider Build();

        IExecuteBuilder RegisterServices(Action<ContainerBuilder> builder);

        IExecuteBuilder MapServices(Action<IContainer> mapper);
    }
}
