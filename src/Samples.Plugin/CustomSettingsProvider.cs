using System;
using Qubit.Xrm.Framework.Abstractions.Configuration;

namespace Samples.Plugin
{
    public class CustomSettingsProvider : ISettingsProvider
    {
        public string Get(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
