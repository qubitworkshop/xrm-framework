using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using PubComp.Caching.Core;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Core
{
    public class User
    {
        private readonly IOrganizationService _organizationService;
        private readonly ICache _cache;

        public User(IWorkflowContext workflowContext, IOrganizationService organizationService, ICache cache)
        {
            _organizationService = organizationService;
            _cache = cache;

            Id = workflowContext.UserId;

            Entity = _organizationService.Retrieve("systemuser", Id, new ColumnSet(false));

            Guard.That(() => Entity).IsNotNull();
        }

        public User(IPluginExecutionContext pluginExecutionContext, IOrganizationService organizationService, ICache cache)
        {
            _organizationService = organizationService;
            _cache = cache;

            Id = pluginExecutionContext.UserId;

            Entity = _organizationService.Retrieve("systemuser", Id, new ColumnSet(false));

            Guard.That(() => Entity).IsNotNull();
        }

        public Guid Id { get; set; }
        public Entity Entity { get; set; }

    }
}
