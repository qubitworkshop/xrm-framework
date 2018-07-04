using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qubit.Xrm.Framework.Abstractions.Logging
{
    public static class FrameworkSinks
    {
        public const string Console = "console";
        public const string File = "file";
        public const string Seq = "seq";
        public const string EventLog = "eventlog";
        public const string SqlServer = "sqlserver";
        public const string Splunk = "splunk";
    }
}
