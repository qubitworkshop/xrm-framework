using Framework.Abstractions.Exceptions.CodeGuard;
using Ninject;

namespace Qubit.Xrm.Framework.Core.CodeActivities
{
    public static class CodeActivityPipelineServiceBuilderExtensions
    {
        public static IKernel AddCodeActivityPipelineServices(this IKernel services)
        {
            Guard.That(() => services).IsNotNull();

            //services.Bind<IApplicationPipelineService>().To<ApplicationPipelineService>().InTransientScope();

            return services;
        }
    }
}
