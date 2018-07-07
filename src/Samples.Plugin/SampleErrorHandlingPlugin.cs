using System;
using Ninject;
using Qubit.Xrm.Framework;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Core;
using Serilog;

namespace Samples.Plugin
{
    [Messages(Messages.Create, Messages.Update)]
    [TargetEntityLogicalName("account")]
    public sealed class SampleErrorHandlingPlugin : Qubit.Xrm.Framework.Plugin
    {
        public SampleErrorHandlingPlugin()
        { }

        public SampleErrorHandlingPlugin(IKernel fakeServices, Action<IKernel> setupMockServices) : base(fakeServices, setupMockServices)
        { }

        public override void Execute(IKernel services)
        {
            throw new Exception("TEST");
        }

        public override void OnError(Exception ex, IKernel services)
        {
            ILogger logger = services.Get<ILogger>();
            logger.Error(ex.Message);
        }
    }
}
