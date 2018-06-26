using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using PubComp.Caching.Core;
using Qubit.Xrm.Framework.Abstractions.Caching;
using Qubit.Xrm.Framework.Core.Plugins;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Core
{
    public class Target
    {
        private readonly IOrganizationService _organizationService;
        private readonly ICache _cache;

        public Target(IWorkflowContext workflowContext, IOrganizationService organizationService, ICache cache)
        {
            _organizationService = organizationService;
            _cache = cache;

            Guid entityId = (Guid) workflowContext.InputParameters["EntityId"];
            Entity = _organizationService.Retrieve(workflowContext.PrimaryEntityName, entityId, new ColumnSet(false));

            Guard.That(() => Entity).IsNotNull();
        }

        public Target(IPluginExecutionContext pluginExecutionContext, IOrganizationService organizationService, ICache cache)
        {
            _organizationService = organizationService;
            _cache = cache;

            object targetInputParameter = pluginExecutionContext.InputParameters["Target"];
            Entity = targetInputParameter as Entity;

            if (Entity == null)
            {
                EntityReference = targetInputParameter as EntityReference;
            }

            if (Entity == null && EntityReference == null)
            {
                throw new InvalidPluginExecutionException("Target could not be resolved. Target is not a known type");
            }
        }

        public Entity Entity { get; set; }
        public EntityReference EntityReference { get; set; }

        public Guid Id => Entity?.Id ?? EntityReference.Id;
        public string EntityLogicalName => Entity?.LogicalName ?? EntityReference.LogicalName;

        public Entity GetEntity(ColumnSet columnSet, bool forceRefresh = false)
        {
            string cacheKey = "TargetEntity".GetCacheKey<PluginExecutionContextAccessor>();

            if (forceRefresh)
            {
                _cache.Clear(cacheKey);
            }

            if (_cache.TryGet(cacheKey, out Entity targetEntity))
            {
                return targetEntity;
            }

            targetEntity = _organizationService.Retrieve(EntityLogicalName, Id, columnSet);
            _cache.Set(cacheKey, targetEntity);
            return targetEntity;
        }

        public T GetEntity<T>(ColumnSet columnSet, bool forceRefresh = false) where T : Entity
        {
            return GetEntity(columnSet, forceRefresh).ToEntity<T>();
        }
    }
}
