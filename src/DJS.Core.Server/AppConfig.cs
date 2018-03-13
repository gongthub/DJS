using Autofac;
using DJS.Core.CPlatform.Address;
using DJS.Core.CPlatform.Transport;
using System.Collections.Generic;

namespace DJS.Core.Server
{
    public class AppConfig
    {
        internal static IpAddressModel SERVERINFO { get; set; }
        internal static ContainerBuilder SERVICES { get; set; }
        internal static IContainer ICONTATINER { get; set; }

        internal static List<IMessageSender> IMSGSENDERLST { get; set; }

        static AppConfig()
        {
            SERVERINFO = new IpAddressModel();
            SERVICES = new ContainerBuilder();
        }
    }
}
