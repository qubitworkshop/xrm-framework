using Microsoft.Xrm.Sdk;
using Ninject;
using Qubit.Xrm.Framework.Core;

namespace Samples.Plugin
{
    [Messages(Messages.Create, Messages.Update)]
    [TargetEntityLogicalName("account")]
    public sealed class SamplePlugin : Qubit.Xrm.Framework.Plugin
    {
        public override void Execute(IKernel services)
        {
            IOrganizationService organizationService = services.Get<IOrganizationService>();
        }
    }
}
