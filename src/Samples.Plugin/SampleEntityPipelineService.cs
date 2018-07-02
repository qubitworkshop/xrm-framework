using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Core.Plugins;
using Serilog;

namespace Samples.Plugin
{
    public class SampleEntityPipelineService : EntityPipelineService, ISampleEntityPipelineService
    {
        private readonly IOrganizationService _organizationService;
        private readonly ISettingsProvider _settingsService;

        public SampleEntityPipelineService(IPluginExecutionContextAccessor executionContextAccessor, ILogger logger,
            IOrganizationService organizationService, ISettingsProvider settingsService) :
            base(executionContextAccessor, logger)
        {
            _organizationService = organizationService;
            _settingsService = settingsService;
        }

        protected override void OnCreated()
        {
            Entity account = ExecutionContextAccessor.Target.GetEntity(new ColumnSet());
            account["qubit_autonumber"] = "12345";
            _organizationService.Update(account);
        }
    }
}
