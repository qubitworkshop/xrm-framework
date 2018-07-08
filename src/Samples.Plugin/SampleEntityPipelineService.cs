using System.Net;
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
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISomeService _someService;

        public SampleEntityPipelineService(IPluginExecutionContextAccessor executionContextAccessor, ILogger logger,
            IOrganizationService organizationService, ISettingsProvider settingsProvider, ISomeService someService) :
            base(executionContextAccessor, logger)
        {
            _organizationService = organizationService;
            _settingsProvider = settingsProvider;
            _someService = someService;
        }

        protected override void OnCreated()
        {
            Entity account = ExecutionContextAccessor.Target.GetEntity(new ColumnSet());
            account["qubit_autonumber"] = "12345";


            using (WebClient client = new WebClient())
            {
                string url = _settingsProvider.Get("abnservice");
                account["qubit_abn"] = client.DownloadString($"{url.TrimEnd('/')}/validate");
            }
                
            _organizationService.Update(account);

            _someService.DoSomethingSpectacular();
        }
    }
}
