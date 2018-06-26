using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Core.Plugins
{
    public static class PluginPipelineServiceBuilderExtensions
    {
        public static IKernel AddPluginPipelineServices(this IKernel services)
        {
            Guard.That(() => services).IsNotNull();

            IEnumerable<Type> pipelineInterfaces = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(e => typeof(IEntityPipelineService).IsAssignableFrom(e) && e.IsInterface)
                .Where(e => e != typeof(IEntityPipelineService));

            foreach (Type pipelineInterface in pipelineInterfaces)
            {
                services.Bind(pipelineInterface)
                    .To(pipelineInterface.GetImplementationType())
                    .InTransientScope();
            }

            return services;
        }

        public static Type GetImplementationType(this Type pipelineInterfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .First(e => pipelineInterfaceType.IsAssignableFrom(e) && !e.IsAbstract && e.IsClass);
        }
    }
}
