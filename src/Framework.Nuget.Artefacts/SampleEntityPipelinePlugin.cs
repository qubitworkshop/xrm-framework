using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Qubit.Xrm.Framework;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Core;

namespace Framework.Nuget.Artefacts
{
    [Messages(Messages.Create, Messages.Update)]
    [TargetEntityLogicalName("account")]
    public sealed class SampleEntityPipelinePlugin : Plugin<CustomSettingsProvider, ISampleEntityPipelineService>
    { }
}
