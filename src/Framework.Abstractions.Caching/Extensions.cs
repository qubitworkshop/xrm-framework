using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Abstractions.Caching
{
    public static class Extensions
    {
        public static string GetCacheKey<TService>(this string key)
        {
            return $"{nameof(TService)}:{key}";
        }
    }
}
