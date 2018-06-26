using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using PubComp.Caching.Core;
using Serilog;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Core.Plugins
{
    public class PluginExecutionContextAccessor : IPluginExecutionContextAccessor
    {
        private readonly ILogger _logger;

        public PluginExecutionContextAccessor(IPluginExecutionContext pluginExecutionContext, ILogger logger, 
            IOrganizationService organizationService, ICache cache)
        {
            _logger = logger;

            Target = new Target(pluginExecutionContext, organizationService, cache);
            User = new User(pluginExecutionContext, organizationService, cache);

            MessageName = pluginExecutionContext.MessageName;
            Stage = (PipelineStages)pluginExecutionContext.Stage;
            CorrelationId = pluginExecutionContext.CorrelationId;
            OperationId = pluginExecutionContext.OperationId;
        }

        public void ValidateExecution(Type implementationType)
        {
            TargetEntityLogicalNameAttribute entityLogicalNameAttribute = Attribute.GetCustomAttribute(implementationType, typeof(TargetEntityLogicalNameAttribute)) as TargetEntityLogicalNameAttribute;

            Guard.That(() => entityLogicalNameAttribute).IsNotNull();

            if (entityLogicalNameAttribute != null && entityLogicalNameAttribute.TargetEntityLogicalName != Target.EntityLogicalName)
            {
                throw new InvalidPluginExecutionException(OperationStatus.Failed, $"Expected target entity was {entityLogicalNameAttribute.TargetEntityLogicalName}, but got {Target.EntityLogicalName}");
            }

            if (Attribute.GetCustomAttribute(implementationType, typeof(MessagesAttribute)) is MessagesAttribute messageAttribute)
            {
                if (messageAttribute.Messages.All(e => e != MessageName))
                {
                    throw new InvalidPluginExecutionException(OperationStatus.Failed, $"Expected message not registered for this plugin, got {MessageName}");
                }
            }
        }

        public Target Target { get; }
        public User User { get; }

        public PipelineStages Stage { get; }
        public string MessageName { get; }

        public Guid CorrelationId { get; }
        public Guid OperationId { get; }

        public void HandleException(Exception ex)
        {
            _logger.Error(ex.Message, ex);
            throw new InvalidPluginExecutionException(ex.Message, ex);
        }
    }
}