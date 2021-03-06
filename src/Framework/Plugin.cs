﻿using System;
using Microsoft.Xrm.Sdk;
using Ninject;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Abstractions.Logging;
using Qubit.Xrm.Framework.Core;
using Qubit.Xrm.Framework.Core.Plugins;

namespace Qubit.Xrm.Framework
{
    public abstract class Plugin<TSettingsProvider> : IPlugin 
        where TSettingsProvider : ISettingsProvider
    {
        private readonly IKernel _fakeServices;
        private readonly Action<IKernel> _setupMockServices;

        protected Plugin()
        { }

        protected Plugin(IKernel fakeServices, Action<IKernel> setupMockServices)
        {
            _fakeServices = fakeServices;
            _setupMockServices = setupMockServices;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            IKernel services = _fakeServices;

            //Fake services were not provided so add actual implemnetations
            if (services == null)
            {
                services = new StandardKernel();
                //Add mockable services here
            }

            //No need to mock these since they are taken care by the mock framework
            services.AddLogging()
                .AddCoreServices()
                .AddPluginServices(serviceProvider)
                .AddPluginPipelineServices();

            services.Bind<ISettingsProvider>().To<TSettingsProvider>().InTransientScope();

            Configure(services);

            _setupMockServices?.Invoke(_fakeServices);

            IPluginExecutionContextAccessor executionContextAccessor = services.Get<IPluginExecutionContextAccessor>();

            try
            {
                PluginExecutionContextAccessor.ValidateExecution(executionContextAccessor, GetType());
                Execute(services);
            }
            catch (Exception ex)
            {
                OnError(ex, services);
            }
        }

        public virtual void Configure(IKernel services)
        { }

        public abstract void Execute(IKernel services);
        public virtual void OnError(Exception ex, IKernel services)
        {
            IPluginExecutionContextAccessor executionContextAccessor = services.Get<IPluginExecutionContextAccessor>();
            executionContextAccessor.HandleException(ex);
        }
    }

    public abstract class Plugin : Plugin<DefaultSettingsProvider>
    {
        protected Plugin()
        { }

        protected Plugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
        { }
    }
}
