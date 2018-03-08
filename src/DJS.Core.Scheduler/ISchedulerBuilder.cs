using Autofac;
using DJS.Core.CPlatform.Scheduler;
using System;

namespace DJS.Core.Scheduler
{
    public interface ISchedulerBuilder
    {
        ISchedulerProvider Build();

        ISchedulerBuilder RegisterServices(Action<ContainerBuilder> builder);

        ISchedulerBuilder MapServices(Action<IContainer> mapper);
    }
}
