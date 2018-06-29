using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Ninject;
using Ninject.Activation;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Abstractions.Logging;
using Serilog;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Services.Logging
{
    public static class FakeLoggingServiceBuilderExtensions
    {
        public static IKernel AddFakeLogging(this IKernel services)
        {
            services.Bind<ILogger>().ToMethod(BuildLogger).InSingletonScope();

            return services;
        }

        private static ILogger BuildLogger(IContext context)
        {
            ISettingsProvider settingsService = context.Kernel.Get<ISettingsProvider>();
            List<LoggerProperty> properties = settingsService.Get<List<LoggerProperty>>("Logging");

            if (context.Kernel.GetBindings(typeof(IPluginExecutionContext)).Any())
            {
                IPluginExecutionContext pluginExecutionContext = context.Kernel.Get<IPluginExecutionContext>();
                properties.Add(new LoggerProperty { Key = "correlationId", Value = pluginExecutionContext.CorrelationId.ToString() });
                properties.Add(new LoggerProperty { Key = "operationId", Value = pluginExecutionContext.OperationId.ToString() });
            }
            else
            {
                IWorkflowContext workflowContext = context.Kernel.Get<IWorkflowContext>();
                properties.Add(new LoggerProperty { Key = "correlationId", Value = workflowContext.CorrelationId.ToString() });
                properties.Add(new LoggerProperty { Key = "operationId", Value = workflowContext.OperationId.ToString() });
            }

            return properties.GetLoggerConfiguration("")
                .WriteTo
                .Console()
                .CreateLogger();
        }
    }
}
