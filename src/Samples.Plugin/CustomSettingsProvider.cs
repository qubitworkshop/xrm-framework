using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using PubComp.Caching.Core;
using Qubit.Xrm.Framework.Abstractions.Configuration;

namespace Samples.Plugin
{
    public class CustomSettingsProvider : DefaultSettingsProvider
    {
        private readonly Dictionary<string, string> _store;

        public CustomSettingsProvider(IOrganizationService organizationService, ICache cache)
            : base(organizationService, cache)
        {
            _store = new Dictionary<string, string>
            {
                { "Logging", "{\"SourceName\": \"XrmFrameworkTests\",\"Sink\": \"Console\" }" }
            };
        }

        public override string Get(string key)
        {
            return _store[key];
        }
    }
}
