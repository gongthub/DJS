using DJS.Core.Scheduler.DependencyResolution;
using DJS.Core.Scheduler.Models;
using DJS.Core.Scheduler.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace DJS.Core.Scheduler
{
    public class AppConfig
    {
        private const string SchedulerSectionName = "SchedulerSettings";
        private readonly SchedulerProvider _schedulerProvider;
        internal static IConfigurationRoot Configuration { get; set; }

        public AppConfig()
        {
            _schedulerProvider = Configuration.Get<SchedulerProvider>();
            RegisterConfigInstance();
        }

        internal static AppConfig DefaultInstance
        {
            get
            {
                var config = ServiceResolver.Current.GetService<AppConfig>();

                if (config == null)
                {
                    config = Activator.CreateInstance(typeof(AppConfig), new object[] { }) as AppConfig;
                    ServiceResolver.Current.Register(null, config);
                }

                return config;
            }
        }

        public T GetContextInstance<T>() where T : class
        {
            var context = ServiceResolver.Current.GetService<T>(typeof(T));
            return context;
        }

        public T GetContextInstance<T>(string name) where T : class
        {
            DebugCheck.NotEmpty(name);
            var context = ServiceResolver.Current.GetService<T>(name);
            return context;
        }

        private void RegisterLocalInstance(string typeName)
        {
            var types = this.GetType().GetTypeInfo().Assembly.GetTypes().Where(p => p.GetTypeInfo().GetInterface(typeName) != null);
            foreach (var t in types)
            {
                var attribute = t.GetTypeInfo().GetCustomAttribute<IdentifySchedulerAttribute>();
                ServiceResolver.Current.Register(attribute.Name.ToString(),
                    Activator.CreateInstance(t));
            }
        }

        private void RegisterConfigInstance()
        {
            var bingingSettings = _schedulerProvider.SchedulerSettings;
            try
            {
                var types =
                    this.GetType().GetTypeInfo()
                        .Assembly.GetTypes()
                        .Where(
                            p => p.GetTypeInfo().GetInterface("ISchedulerProvider") != null);
                foreach (var t in types)
                {
                    var attribute = t.GetTypeInfo().GetCustomAttribute<IdentifySchedulerAttribute>();
                    if (attribute != null)
                        ServiceResolver.Current.Register(attribute.Name.ToString(),
                            Activator.CreateInstance(t));
                }
            }
            catch { }
        }
    }
}
