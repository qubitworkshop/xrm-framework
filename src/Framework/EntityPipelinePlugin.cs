using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Core;

namespace Qubit.Xrm.Framework
{
    public abstract class EntityPipelinePlugin<TSettingsProvider, TPipelineServiceInterface> : Plugin<TSettingsProvider>
        where TSettingsProvider : ISettingsProvider
        where TPipelineServiceInterface : IEntityPipelineService
    {
        protected EntityPipelinePlugin()
        { }

        protected EntityPipelinePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
        { }

        public override void Execute(IKernel services)
        {
            TPipelineServiceInterface entityPipelineService = services.Get<TPipelineServiceInterface>();
            entityPipelineService.HandlePipelineEvent();
        }
    }

    public abstract class
        EntityPipelinePlugin<TPipelineServiceInterface> : EntityPipelinePlugin<DefaultSettingsProvider,
            TPipelineServiceInterface>
        where TPipelineServiceInterface : IEntityPipelineService
    {
        protected EntityPipelinePlugin()
        { }

        protected EntityPipelinePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
        { }
    }
}
