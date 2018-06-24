using Ninject;
using Ninject.Activation;
using PubComp.Caching.Core;
using PubComp.Caching.SystemRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Abstractions.Caching
{
    public static class CachingServiceBuilderExtensions
    {
        public static IKernel AddInMemoryCaching(this IKernel services)
        {
            services.Bind<ICache>().ToMethod(BuildInMemoryCache).InSingletonScope();
            return services;
        }

        private static ICache BuildInMemoryCache(IContext context)
        {
            return new InMemoryCache("PluginCache", new TimeSpan(0, 1, 0));
        }
    }
}
