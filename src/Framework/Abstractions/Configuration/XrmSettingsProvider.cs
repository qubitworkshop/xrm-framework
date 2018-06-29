using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qubit.Xrm.Framework.Abstractions.Configuration
{
    public class XrmSettingsProvider : ISettingsProvider
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
