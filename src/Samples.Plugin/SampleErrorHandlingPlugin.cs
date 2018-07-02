using System;
using Ninject;
using Qubit.Xrm.Framework;
using Qubit.Xrm.Framework.Core;

namespace Samples.Plugin
{
    [Messages(Messages.Create, Messages.Update)]
    [TargetEntityLogicalName("account")]
    public sealed class SampleErrorHandlingPlugin : Plugin<CustomSettingsProvider>
    {
        public override void Execute(IKernel services)
        {
            throw new Exception("some exception");
        }

        public override void OnError(Exception ex, IKernel services)
        {
            //handle exception here
            base.OnError(ex, services);
        }
    }
}
