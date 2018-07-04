using System;
using Ninject;
using Qubit.Xrm.Framework;
using Qubit.Xrm.Framework.Core;

namespace Samples.Plugin
{
    [Messages(Messages.Create, Messages.Update)]
    [TargetEntityLogicalName("account")]
    public sealed class SampleEntityPipelinePlugin : EntityPipelinePlugin<ISampleEntityPipelineService>
    {
        public SampleEntityPipelinePlugin()
        { }

        public SampleEntityPipelinePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
        { }
    }
}
