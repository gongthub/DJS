using Autofac;
using DJS.Core.CPlatform.Address;

namespace DJS.Core.Scheduler
{
    public class AppConfig
    {
        internal static IpAddressModel SERVERINFO { get; set; }
        internal static ContainerBuilder SERVICES { get; set; }
        internal static IContainer ICONTATINER { get; set; }

        static AppConfig()
        {
            SERVERINFO = new IpAddressModel();
            SERVICES = new ContainerBuilder();
        }


        
    }
}
