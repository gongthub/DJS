using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.Core.ServiceHosting.Startup.Implementation
{
    public  class DelegateStartup : StartupBase<ContainerBuilder>
    {
        private Action<IContainer> _configureApp;

        public DelegateStartup(Action<IContainer> configureApp) : base()
        {
            _configureApp = configureApp;
        }

        public override void Configure(IContainer app) => _configureApp(app);
    }
}
