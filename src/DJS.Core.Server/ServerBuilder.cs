using Autofac;
using DJS.Core.CPlatform.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.Server
{
    public class ServerBuilder : IServerBuilder
    {
        private readonly List<Action<ContainerBuilder>> _registerServicesDelegates;
        private readonly List<Action<IContainer>> _mapServicesDelegates;


        public ServerBuilder()
        {
            _registerServicesDelegates = new List<Action<ContainerBuilder>>();
            _mapServicesDelegates = new List<Action<IContainer>>();
        }

        public IServerProvider Build()
        {
            var services = RegisterServices();
            var iContainer = services.Build();
            AppConfig.SERVICES = services;
            AppConfig.ICONTATINER = iContainer;
            var communicationProvider = iContainer.Resolve<IServerProvider>();
            return communicationProvider;
        }

        public IServerBuilder MapServices(Action<IContainer> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapServicesDelegates.Add(mapper);
            return this;
        }

        public IServerBuilder RegisterServices(Action<ContainerBuilder> builder)
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
