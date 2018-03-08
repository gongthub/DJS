using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DJS.UnitTest
{
    [TestClass]
    public class TestDemo
    {

        [TestMethod]
        public void DoTest()
        {
            var con = new ServiceBuilder(new ContainerBuilder())
               .UseDemo();
            var c = con.containerBuilder.Build();
            bool bstate = c.IsRegisteredWithName<ITest>("TestD1");
            bool  bstate2 = c.IsRegisteredWithKey<ITest>("TestD1");
            var it1 = c.ResolveKeyed<ITest>("TestD1");
            var it2 = c.ResolveKeyed<ITest>("TestD2");
        }

    }

    public static class ServiceHuilerE
    {

        /// <summary>
        /// 使用DotNetty进行传输。
        /// </summary>
        /// <param name="builder">服务构建者。</param>
        /// <returns>服务构建者。</returns>
        public static IServiceBuilder UseDemo(this IServiceBuilder builder)
        {
            var services = builder.containerBuilder;
            services.RegisterType(typeof(TestD1)).As(typeof(ITest)).Keyed<ITest>("TestD1").SingleInstance();
            services.RegisterType(typeof(TestD2)).As(typeof(ITest)).Keyed<ITest>("TestD2").SingleInstance();
            return builder;
        }
    }
    public interface ITest
    {
        void run();
    }

    public class TestD1 : ITest
    {
        public void run()
        {
            throw new NotImplementedException();
        }
    }
    public class TestD2 : ITest
    {
        public void run()
        {
            throw new NotImplementedException();
        }
    }
}
