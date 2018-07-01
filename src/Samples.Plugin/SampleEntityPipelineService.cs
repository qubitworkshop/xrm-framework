﻿using Microsoft.Xrm.Sdk;
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
    }
}
