using System;
using System.Activities;
using Framework.Abstractions.Caching;
using Framework.Abstractions.Exceptions.CodeGuard;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Ninject;
using Qubit.Xrm.Framework.Core;
using Qubit.Xrm.Framework.Core.CodeActivities;
using Qubit.Xrm.Framework.Core.Plugins;

namespace Qubit.Xrm.Framework
{
    public static class CoreServiceBuilderExtensions
    {
        public static IKernel AddCoreServices(this IKernel services)
        {
            Guard.That(() => services).IsNotNull();

            services.AddInMemoryCaching();

            return services;
        }

        public static IKernel AddCodeActivityServices(this IKernel services, CodeActivityContext codeActivityContext)
        {
            Guard.That(() => codeActivityContext).IsNotNull();
            Guard.That(() => services).IsNotNull();

            services.Bind<CodeActivityContext>().ToConstant(codeActivityContext).InSingletonScope();

            IWorkflowContext executionContext = codeActivityContext.GetExtension<IWorkflowContext>();
            services.Bind<IWorkflowContext>().ToConstant(executionContext).InTransientScope();

            ITracingService tracingService = codeActivityContext.GetExtension<ITracingService>();
            services.Bind<ITracingService>().ToConstant(tracingService).InTransientScope();

            IOrganizationServiceFactory organizationServiceFactory = codeActivityContext.GetExtension<IOrganizationServiceFactory>();
            services.Bind<IOrganizationServiceFactory>().ToConstant(organizationServiceFactory).InTransientScope();

            services.Bind<IOrganizationService>().ToMethod(context =>
            {
                IOrganizationServiceFactory serviceFactory = context.Kernel.Get<IOrganizationServiceFactory>();
                return serviceFactory.CreateOrganizationService(null);
            });

            services.Bind<ICodeActivityExecutionContextAccessor>().To<CodeActivityExecutionContextAccessor>().InTransientScope();

            services.Bind<User>().ToMethod(context =>
            {
                ICodeActivityExecutionContextAccessor executionContextAccessor = context.Kernel.Get<ICodeActivityExecutionContextAccessor>();
                return executionContextAccessor.User;
            });

            return services;
        }

        public static IKernel AddPluginServices(this IKernel services, IServiceProvider serviceProvider)
        {
            Guard.That(() => serviceProvider).IsNotNull();
            Guard.That(() => services).IsNotNull();

            services.Bind<IServiceProvider>().ToConstant(serviceProvider).InSingletonScope();

            IPluginExecutionContext executionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            services.Bind<IPluginExecutionContext>().ToConstant(executionContext).InTransientScope();

            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            services.Bind<ITracingService>().ToConstant(tracingService).InTransientScope();

            IOrganizationServiceFactory organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            services.Bind<IOrganizationServiceFactory>().ToConstant(organizationServiceFactory).InTransientScope();

            services.Bind<IOrganizationService>().ToMethod(context =>
            {
                IOrganizationServiceFactory serviceFactory = context.Kernel.Get<IOrganizationServiceFactory>();
                return serviceFactory.CreateOrganizationService(null);
            });

            services.Bind<IPluginExecutionContextAccessor>().To<PluginExecutionContextAccessor>().InTransientScope();

            services.Bind<User>().ToMethod(context =>
            {
                IPluginExecutionContextAccessor executionContextAccessor = context.Kernel.Get<IPluginExecutionContextAccessor>();
                return executionContextAccessor.User;
            });

            return services;
        }
    }
}
