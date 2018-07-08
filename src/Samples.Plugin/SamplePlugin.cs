using System;
using Microsoft.Xrm.Sdk;
using Ninject;
using Qubit.Xrm.Framework.Core;

namespace Samples.Plugin
{
    [Messages(Messages.Create, Messages.Update)]
    [TargetEntityLogicalName("account")]
    public sealed class SamplePlugin : Qubit.Xrm.Framework.Plugin
    {
        public SamplePlugin()
        { }

        public SamplePlugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
        { }

        public override void Execute(IKernel services)
        {
            IOrganizationService organizationService = services.Get<IOrganizationService>();
        }
    }
}
