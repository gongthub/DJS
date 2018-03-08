using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.UnitTest
{
    public interface IServiceBuilder
    {
        ContainerBuilder containerBuilder { get; set; }
    }
    /// <summary>
    /// 默认服务构建者。
    /// </summary>
    internal sealed class ServiceBuilder : IServiceBuilder
    {
        public ServiceBuilder(ContainerBuilder services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            containerBuilder = services;
        }

        #region Implementation of IServiceBuilder

        /// <summary>
        /// 服务集合。
        /// </summary>
        public ContainerBuilder containerBuilder { get; set; }

        #endregion Implementation of IServiceBuilder
    }
}
