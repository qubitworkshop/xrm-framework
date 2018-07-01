using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Ninject;
using Qubit.Xrm.Framework;
using Qubit.Xrm.Framework.Core;

namespace Framework.Nuget.Artefacts
{
    [Messages(Messages.Create, Messages.Update)]
    [TargetEntityLogicalName("account")]
    public sealed class SamplePlugin : Plugin
    {
        public override void Execute(IKernel services)
        {
            IOrganizationService organizationService = services.Get<IOrganizationService>();
        }
    }
}
