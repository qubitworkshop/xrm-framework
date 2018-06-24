using System;
using Microsoft.Xrm.Sdk;

namespace Framework.Core
{
    public static class CoreExtensions
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
