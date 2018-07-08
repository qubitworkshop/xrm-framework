using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qubit.Xrm.Framework.Helpers
{
    public static class DateTimeExtensions
    {
        public static string ToUtcFormat(this DateTime date)
        {
            return new DateTime(date.Year,
                date.Month,
                date.Day,
                date.Hour,
                date.Minute,
                date.Second,
                DateTimeKind.Utc).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
        }
    }
}
