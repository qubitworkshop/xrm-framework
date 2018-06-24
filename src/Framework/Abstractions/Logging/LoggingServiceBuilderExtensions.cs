using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Abstractions.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Ninject;
using Ninject.Activation;
using Serilog;

namespace Framework.Abstractions.Logging
{
    public static class LoggingServiceBuilderExtensions
    {
        public static IKernel AddLogging(this IKernel services)
        {
            services.Bind<ILogger>().ToMethod(BuildLogger).InSingletonScope();

            return services;
        }

        private static ILogger BuildLogger(IContext context)
        {
            ISettingsService settingsService = context.Kernel.Get<ISettingsService>();
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

        public static LoggerConfiguration GetLoggerConfiguration(this IEnumerable<LoggerProperty> properties, string name)
        {
            Dictionary<string, string> propertiesDictionary =
                properties.ToDictionary(loggerProperty => loggerProperty.Key, loggerProperty => loggerProperty.Value);

            return new LoggerConfiguration()
                .ReadFrom.KeyValuePairs(propertiesDictionary)
                .Enrich.WithProperty("Service Type", name)
                .Enrich.WithProperty("Operation Id", propertiesDictionary["operationId"])
                .Enrich.WithProperty("Correlation Id", propertiesDictionary["correlationId"]);
        }
    }
}
