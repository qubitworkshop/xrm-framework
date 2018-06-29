using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qubit.Xrm.Framework.Abstractions.Configuration;

namespace Framework.Nuget.Artefacts
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
