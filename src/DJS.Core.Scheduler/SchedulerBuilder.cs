﻿using Autofac;
using DJS.Core.CPlatform.Scheduler;
using System;
using System.Collections.Generic;

namespace DJS.Core.Scheduler
{
    public class SchedulerBuilder : ISchedulerBuilder
    {
        private readonly List<Action<ContainerBuilder>> _registerServicesDelegates;
        private readonly List<Action<IContainer>> _mapServicesDelegates;
       

        public SchedulerBuilder()
        {
            _registerServicesDelegates = new List<Action<ContainerBuilder>>();
            _mapServicesDelegates = new List<Action<IContainer>>();
        }

        public ISchedulerProvider Build()
        {
            var services = RegisterServices();
            var iContainer = services.Build();
            AppConfig.SERVICES = services;
            AppConfig.ICONTATINER = iContainer;
            var schedulerProvider = iContainer.Resolve<ISchedulerProvider>();
            return schedulerProvider;
        }

        public ISchedulerBuilder MapServices(Action<IContainer> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapServicesDelegates.Add(mapper);
            return this;
        }

        public ISchedulerBuilder RegisterServices(Action<ContainerBuilder> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _registerServicesDelegates.Add(builder);
            return this;
        }
        
        private ContainerBuilder RegisterServices()
        {
            var hostingServices = new ContainerBuilder();
            foreach (var registerServices in _registerServicesDelegates)
            {
                registerServices(hostingServices);
            }
            return hostingServices;
        }
    }
}
