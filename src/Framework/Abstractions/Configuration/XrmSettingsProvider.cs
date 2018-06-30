﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using PubComp.Caching.Core;
using Qubit.Xrm.Framework.Abstractions.Caching;
using Qubit.Xrm.Framework.Helpers;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Abstractions.Configuration
{
    public class XrmSettingsProvider : ISettingsProvider
    {
        private readonly IOrganizationService _organizationService;
        private readonly ICache _cache;

        public XrmSettingsProvider(IOrganizationService organizationService, ICache cache)
        {
            _organizationService = organizationService;
            _cache = cache;
        }

        public string Get(string key)
        {
            string cacheKey = key.GetCacheKey<XrmSettingsProvider>();

            if (_cache.TryGet(cacheKey, out string value))
            {
                return value;
            }

            Entity settingRecord = EnsureSetting(key);

            _cache.Set(cacheKey, settingRecord["qubit_value"].ToString().RemoveInvalidCharacters());

            return Get(key);

        }

        public T Get<T>(string key) where T : class
        {
            string cacheKey = $"{key.GetCacheKey<XrmSettingsProvider>()}:{typeof(T).FullName}";
            if (_cache.TryGet(cacheKey, out T value))
            {
                return value;
            }

            string jsonString = Get(key);
            value = JsonConvert.DeserializeObject<T>(jsonString);
            _cache.Set(cacheKey, value);

            return Get<T>(key);
        }

        private Entity EnsureSetting(string key)
        {
            QueryExpression query = new QueryExpression("qubit_settings")
            {
                ColumnSet = new ColumnSet("qubit_value"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("qubit_key", ConditionOperator.Equal, key)
                    }
                }
            };

            EntityCollection resultSet = _organizationService.RetrieveMultiple(query);

            if (!resultSet.Entities.Any())
            {
                Entity newSetting = new Entity("qubit_settings")
                {
                    ["qubit_key"] = key
                };

                _organizationService.Create(newSetting);
                return EnsureSetting(key);
            }

            Guard.That(() => resultSet.Entities.Count).IsEqual(1);

            return resultSet.Entities.First();
        }
    }
}
