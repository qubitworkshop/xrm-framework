using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using PubComp.Caching.Core;
using Serilog;

namespace Qubit.Xrm.Framework.Core.CodeActivities
{
    public class CodeActivityExecutionContextAccessor : ICodeActivityExecutionContextAccessor
    {
        private readonly IWorkflowContext _workflowContext;
        private readonly ILogger _logger;

        public CodeActivityExecutionContextAccessor(IWorkflowContext workflowContext, ILogger logger,
            IOrganizationService organizationService, ICache cache)
        {
            _workflowContext = workflowContext;
            _logger = logger;

            Target = new Target(workflowContext, organizationService, cache);
            User = new User(workflowContext, organizationService, cache);

            StageName = workflowContext.StageName;
            CorrelationId = workflowContext.CorrelationId;
            OperationId = workflowContext.OperationId;
        }

        public string StageName { get; }

        public Target Target { get; }

        public Guid CorrelationId { get; }
        public Guid OperationId { get; }

        public User User { get; }

        public void HandleException(Exception ex)
        {
            _logger.Error(ex.Message, ex);
            throw new InvalidPluginExecutionException(OperationStatus.Failed, ex.ToString());
        }

        public void ValidateExecution(Type implementationType)
        {
            
        }
    }
}
