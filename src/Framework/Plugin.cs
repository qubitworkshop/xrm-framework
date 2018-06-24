﻿using System;
using Framework.Abstractions.Logging;
using Microsoft.Xrm.Sdk;
using Ninject;
using Qubit.Xrm.Framework.Core.Plugins;

namespace Qubit.Xrm.Framework
{
    public abstract class Plugin : IPlugin 
    {
        private readonly IKernel _fakeServices;

        protected Plugin()
        { }

        protected Plugin(IKernel fakeServices)
        {
            _fakeServices = fakeServices;
        }

        public void Execute(IServiceProvider serviceProvider)
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
                .AddPluginServices(serviceProvider)
                .AddPluginPipelineServices();

            IPluginExecutionContextAccessor executionContextAccessor = services.Get<IPluginExecutionContextAccessor>();

            try
            {
                executionContextAccessor.ValidateExecution(GetType());
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
            IPluginExecutionContextAccessor executionContextAccessor = services.Get<IPluginExecutionContextAccessor>();
            executionContextAccessor.HandleException(ex);
        }
    }
}
