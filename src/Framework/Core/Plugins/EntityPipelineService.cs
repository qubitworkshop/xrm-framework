using System.Collections.Generic;
using Serilog;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Core.Plugins
{
    public abstract class EntityPipelineService : IEntityPipelineService
    {
        protected IPluginExecutionContextAccessor ExecutionContextAccessor { get; }
        private readonly ILogger _logger;

        protected EntityPipelineService(IPluginExecutionContextAccessor executionContextAccessor, ILogger logger)
        {
            ExecutionContextAccessor = executionContextAccessor;
            _logger = logger;
        }

        public void HandlePipelineEvent()
        {
            _logger.Verbose($"Handling pipeline for entity {ExecutionContextAccessor.Target.EntityLogicalName} with id {ExecutionContextAccessor.Target.Id}");

            Guard.That(() => ExecutionContextAccessor.Stage).IsNotEqual(PipelineStages.Invalid);
            Guard.That(() => ExecutionContextAccessor.Stage).IsOneOf(new List<PipelineStages> { PipelineStages.PreValidation, PipelineStages.PreOperation, PipelineStages.PostOperation });

            switch (ExecutionContextAccessor.MessageName)
            {
                case Messages.Create:
                    OnCreateEvent();
                    break;
                case Messages.Update:
                    OnUpdateEvent();
                    break;
                case Messages.Delete:
                    OnDeleteEvent();
                    break;
                case Messages.Send:
                    OnSendEvent();
                    break;
            }
        }

        private void OnSendEvent()
        {
            switch (ExecutionContextAccessor.Stage)
            {
                case PipelineStages.PreValidation:
                    BeforeSendingBeforeValidation();
                    break;
                case PipelineStages.PreOperation:
                    BeforeSending();
                    break;
                case PipelineStages.PostOperation:
                    OnSent();
                    break;
                default:
                    throw new InvalidPipelineExecutionException("Unknown Pipeline event stage");
            }
        }

        private void OnCreateEvent()
        {
            switch (ExecutionContextAccessor.Stage)
            {
                case PipelineStages.PreValidation:
                    BeforeCreatingBeforeValidation();
                    break;
                case PipelineStages.PreOperation:
                    BeforeCreating();
                    break;
                case PipelineStages.PostOperation:
                    OnCreated();
                    break;
                default:
                    throw new InvalidPipelineExecutionException("Unknown Pipeline event stage");
            }
        }

        private void OnUpdateEvent()
        {
            switch (ExecutionContextAccessor.Stage)
            {
                case PipelineStages.PreValidation:
                    BeforeUpdatingBeforeValidation();
                    break;
                case PipelineStages.PreOperation:
                    BeforeUpdating();
                    break;
                case PipelineStages.PostOperation:
                    OnUpdated();
                    break;
                default:
                    throw new InvalidPipelineExecutionException("Unknown Pipeline event stage");
            }
        }

        private void OnDeleteEvent()
        {
            switch (ExecutionContextAccessor.Stage)
            {
                case PipelineStages.PreValidation:
                    BeforeDeletingBeforeValidation();
                    break;
                case PipelineStages.PreOperation:
                    BeforeDeleting();
                    break;
                case PipelineStages.PostOperation:
                    OnDeleted();
                    break;
                default:
                    throw new InvalidPipelineExecutionException("Unknown Pipeline event stage");
            }
        }

        //SEND
        protected virtual void BeforeSendingBeforeValidation()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void BeforeSending()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void OnSent()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        //CREATEs
        protected virtual void BeforeCreatingBeforeValidation()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void BeforeCreating()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void OnCreated()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        //UPDATE
        protected virtual void BeforeUpdatingBeforeValidation()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void BeforeUpdating()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void OnUpdated()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        //DELETE
        protected virtual void BeforeDeletingBeforeValidation()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void BeforeDeleting()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }

        protected virtual void OnDeleted()
        {
            throw new InvalidPipelineExecutionException("Pipeline event not implemented");
        }
    }
}
