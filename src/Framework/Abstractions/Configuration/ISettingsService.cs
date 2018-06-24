using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Abstractions.Configuration
{
    public interface ISettingsService
    {
        string Get(string key);
        T Get<T>(string key) where T : class;
    }
}
