using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Ninject;
using Ninject.Activation;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Serilog;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Abstractions.Logging
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
            ISettingsProvider settingsService = context.Kernel.Get<ISettingsProvider>();
            Dictionary<string, string> properties = settingsService.Get<Dictionary<string, string>>("Logging");

            if (context.Kernel.GetBindings(typeof(IPluginExecutionContext)).Any())
            {
                IPluginExecutionContext pluginExecutionContext = context.Kernel.Get<IPluginExecutionContext>();
                properties.Add("CorrelationId", pluginExecutionContext.CorrelationId.ToString());
                properties.Add("OperationId", pluginExecutionContext.OperationId.ToString());
            }
            else
            {
                IWorkflowContext workflowContext = context.Kernel.Get<IWorkflowContext>();
                properties.Add("CorrelationId", workflowContext.CorrelationId.ToString());
                properties.Add("OperationId", workflowContext.OperationId.ToString());
            }

            Guard.That(properties.ContainsKey("SourceName")).IsTrue();
            Guard.That(properties.ContainsKey("Sink")).IsTrue();

            string sourceName = properties["SourceName"];

            switch (properties["Sink"].ToLower())
            {
                case FrameworkSinks.Console:
                    return properties.GetLoggerConfiguration(sourceName)
                        .WriteTo
                        .Console()
                        .CreateLogger();
                case FrameworkSinks.File:
                    Guard.That(properties.ContainsKey("Path")).IsTrue();
                    return properties.GetLoggerConfiguration(sourceName)
                        .WriteTo
                        .File(properties["Path"])
                        .CreateLogger();
                case FrameworkSinks.Seq:
                    Guard.That(properties.ContainsKey("ServerUrl")).IsTrue();
                    return properties.GetLoggerConfiguration(sourceName)
                        .WriteTo
                        .Seq(properties["ServerUrl"])
                        .CreateLogger();
                case FrameworkSinks.EventLog:
                    return properties.GetLoggerConfiguration(sourceName)
                        .WriteTo
                        .EventLog(sourceName)
                        .CreateLogger();
                case FrameworkSinks.SqlServer:
                    Guard.That(properties.ContainsKey("ConnectionString")).IsTrue();
                    Guard.That(properties.ContainsKey("TableName")).IsTrue();
                    return properties.GetLoggerConfiguration(sourceName)
                        .WriteTo
                        .MSSqlServer(properties["ConnectionString"], 
                            properties["TableName"],
                            autoCreateSqlTable: true)
                        .CreateLogger();
                case FrameworkSinks.Splunk:
                    Guard.That(properties.ContainsKey("EventCollectorUrl")).IsTrue();
                    Guard.That(properties.ContainsKey("Token")).IsTrue();
                    return properties.GetLoggerConfiguration(sourceName)
                        .WriteTo
                        .EventCollector(properties["EventCollectorUrl"], 
                            properties["Token"])
                        .CreateLogger();
                default:
                    return properties.GetLoggerConfiguration(sourceName)
                        .WriteTo
                        .Console()
                        .CreateLogger();
            }

        }

        public static LoggerConfiguration GetLoggerConfiguration(this Dictionary<string, string> properties, string name)
        {
            Guard.That(properties.ContainsKey("OperationId")).IsTrue();
            Guard.That(properties.ContainsKey("CorrelationId")).IsTrue();

            return new LoggerConfiguration()
                .ReadFrom.KeyValuePairs(properties)
                .Enrich.WithProperty("Service Type", name)
                .Enrich.WithProperty("Operation Id", properties["OperationId"])
                .Enrich.WithProperty("Correlation Id", properties["CorrelationId"]);
        }
    }
}
