using System;
using System.Activities;
using Ninject;
using Qubit.Xrm.Framework.Abstractions.Logging;
using Qubit.Xrm.Framework.Core.CodeActivities;

namespace Qubit.Xrm.Framework
{
    public abstract class WorkflowActivity : CodeActivity
    {
        private readonly IKernel _fakeServices;

        protected WorkflowActivity()
        { }

        protected WorkflowActivity(IKernel fakeServices)
        {
            _fakeServices = fakeServices;
        }

        protected override void Execute(CodeActivityContext context)
        {
            IKernel services = _fakeServices;

            //Fake services were not provided so add actual implemnetations
            if (services == null)
            {
                services = new StandardKernel();

                //Add mockable services here
                services.AddLogging();
            }

            //No need to mock these since they are taken care by the mock framework
            services.AddCoreServices()
                .AddCodeActivityServices(context)
                .AddCodeActivityPipelineServices();

            ICodeActivityExecutionContextAccessor executionContextAccessor = services.Get<ICodeActivityExecutionContextAccessor>();

            try
            {
                CodeActivityExecutionContextAccessor.ValidateExecution(executionContextAccessor, GetType());
                Execute(services);
            }
            catch (Exception ex)
            {
                OnError(ex, services);
            }

        }

        public abstract void Execute(IKernel services);
        public virtual void OnError(Exception ex, IKernel services)
        {
            ICodeActivityExecutionContextAccessor executionContextAccessor = services.Get<ICodeActivityExecutionContextAccessor>();
            executionContextAccessor.HandleException(ex);
        }
    }
}
