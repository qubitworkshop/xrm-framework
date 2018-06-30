using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qubit.Xrm.Framework.Abstractions.Logging
{
    public class SettingsKeyNotFoundException : Exception
    {
        public SettingsKeyNotFoundException(string key) 
            : base($"the key '{key}' was not in the logging settings. Please make sure this key is present and has a valid value")
        { }
    }
}
