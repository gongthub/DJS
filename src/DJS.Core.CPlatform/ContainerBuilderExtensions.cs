using Autofac;
using DJS.Core.CPlatform.Module;
using DJS.Core.CPlatform.Serialization;
using DJS.Core.CPlatform.Serialization.Implementation;
using DJS.Core.CPlatform.Server;
using DJS.Core.CPlatform.Server.Implementation;
using DJS.Core.CPlatform.Transport.Codec;
using DJS.Core.CPlatform.Transport.Codec.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DJS.Core.CPlatform
{
    /// <summary>
    /// 服务构建者。
    /// </summary>
    public interface IServiceBuilder
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        ContainerBuilder Services { get; set; }
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
            Services = services;
        }

        #region Implementation of IServiceBuilder

        /// <summary>
        /// 服务集合。
        /// </summary>
        public ContainerBuilder Services { get; set; }

        #endregion Implementation of IServiceBuilder
    }

    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 添加服务。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <returns>服务构建者。</returns>
        public static void AddServices(this ContainerBuilder builder, Action<IServiceBuilder> option)
        {
            option.Invoke(builder.AddServices());
        }
        /// <summary>
        /// 添加服务。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <returns>服务构建者。</returns>
        private static IServiceBuilder AddServices(this ContainerBuilder services)
        {
            return new ServiceBuilder(services)
                .AddJsonSerialization()
                .UseJsonCodec();
        }
        /// <summary>
        /// 添加Json序列化支持。
        /// </summary>
        /// <param name="builder">服务构建者。</param>
        /// <returns>服务构建者。</returns>
        public static IServiceBuilder AddJsonSerialization(this IServiceBuilder builder)
        {
            var services = builder.Services;
            services.RegisterType(typeof(JsonSerializer)).As(typeof(ISerializer<string>)).SingleInstance();
            services.RegisterType(typeof(StringByteArraySerializer)).As(typeof(ISerializer<byte[]>)).SingleInstance();
            services.RegisterType(typeof(StringObjectSerializer)).As(typeof(ISerializer<object>)).SingleInstance();
            return builder;
        }
        #region Codec Factory

        /// <summary>
        /// 使用编解码器。
        /// </summary>
        /// <param name="builder">服务构建者。</param>
        /// <param name="codecFactory"></param>
        /// <returns>服务构建者。</returns>
        public static IServiceBuilder UseCodec(this IServiceBuilder builder, ITransportMessageCodecFactory codecFactory)
        {
            builder.Services.RegisterInstance(codecFactory).SingleInstance();
            return builder;
        }

        /// <summary>
        /// 使用编解码器。
        /// </summary>
        /// <typeparam name="T">编解码器工厂实现类型。</typeparam>
        /// <param name="builder">服务构建者。</param>
        /// <returns>服务构建者。</returns>
        public static IServiceBuilder UseCodec<T>(this IServiceBuilder builder) where T : class, ITransportMessageCodecFactory
        {
            builder.Services.RegisterType(typeof(T)).As(typeof(ITransportMessageCodecFactory)).SingleInstance();
            return builder;
        }

        /// <summary>
        /// 使用编解码器。
        /// </summary>
        /// <param name="builder">服务构建者。</param>
        /// <param name="codecFactory">编解码器工厂创建委托。</param>
        /// <returns>服务构建者。</returns>
        public static IServiceBuilder UseCodec(this IServiceBuilder builder, Func<IServiceProvider, ITransportMessageCodecFactory> codecFactory)
        {
            builder.Services.RegisterAdapter(codecFactory).SingleInstance();
            return builder;
        }

        #endregion Codec Factory

        /// <summary>
        /// 使用Json编解码器。
        /// </summary>
        /// <param name="builder">服务构建者。</param>
        /// <returns>服务构建者。</returns>
        public static IServiceBuilder UseJsonCodec(this IServiceBuilder builder)
        {
            return builder.UseCodec<JsonTransportMessageCodecFactory>();
        }


        private static List<AbstractModule> GetAbstractModules(Assembly assembly)
        {
            var abstractModules = new List<AbstractModule>();
            Type[] arrayModule =
                assembly.GetTypes().Where(
                    t => t.IsSubclassOf(typeof(AbstractModule))).ToArray();
            foreach (var moduleType in arrayModule)
            {
                var abstractModule = (AbstractModule)Activator.CreateInstance(moduleType);
                abstractModules.Add(abstractModule);
            }
            return abstractModules;
        }
    }
}