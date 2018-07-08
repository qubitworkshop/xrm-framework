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
    /// <summary>
    /// Provides information about the target context entity in the current execution context
    /// </summary>
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

        /// <summary>
        /// The target entity if the target is a reference of type Entity
        /// </summary>
        public Entity Entity { get; set; }

        /// <summary>
        /// The target entity reference if the target is a reference of type EntityReference
        /// </summary>
        public EntityReference EntityReference { get; set; }

        /// <summary>
        /// The Id of the target entity
        /// </summary>
        public Guid Id => Entity?.Id ?? EntityReference.Id;

        /// <summary>
        /// The target entity logical name
        /// </summary>
        public string EntityLogicalName => Entity?.LogicalName ?? EntityReference.LogicalName;


        /// <summary>
        /// Gets the target entity from CRM
        /// </summary>
        /// <param name="columnSet">The columns to retrieve</param>
        /// <param name="forceRefresh">Mark as true if you want to query CRM for each call and not get from the cache</param>
        /// <returns>The entity with the attributes specified</returns>
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

        /// <summary>
        /// Same as non generic GetEntity but cast to a early bound type
        /// </summary>
        /// <typeparam name="T">The poroxy type</typeparam>
        /// <param name="columnSet">The columns to retrieve</param>
        /// <param name="forceRefresh">Mark as true if you want to query CRM for each call and not get from the cache</param>
        /// <returns>The early bound entity with the attributes specified</returns>
        public T GetEntity<T>(ColumnSet columnSet, bool forceRefresh = false) where T : Entity
        {
            return GetEntity(columnSet, forceRefresh).ToEntity<T>();
        }
    }
}
