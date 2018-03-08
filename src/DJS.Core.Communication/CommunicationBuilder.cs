using Autofac;
using DJS.Core.CPlatform.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Communication
{
    public class CommunicationBuilder : ICommunicationBuilder
    {
        private readonly List<Action<ContainerBuilder>> _registerServicesDelegates;
        private readonly List<Action<IContainer>> _mapServicesDelegates;


        public CommunicationBuilder()
        {
            _registerServicesDelegates = new List<Action<ContainerBuilder>>();
            _mapServicesDelegates = new List<Action<IContainer>>();
        }

        public ICommunicationProvider Build()
        {
            var services = RegisterServices();
            var iContainer = services.Build();
            AppConfig.SERVICES = services;
            AppConfig.ICONTATINER = iContainer;
            var communicationProvider = iContainer.Resolve<ICommunicationProvider>();
            return communicationProvider;
        }

        public ICommunicationBuilder MapServices(Action<IContainer> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapServicesDelegates.Add(mapper);
            return this;
        }

        public ICommunicationBuilder RegisterServices(Action<ContainerBuilder> builder)
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
