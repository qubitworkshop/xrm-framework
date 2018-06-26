using System;

namespace Qubit.Xrm.Framework.Abstractions.Logging
{
    [Serializable]
    public class LoggerProperty
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
