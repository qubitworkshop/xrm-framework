using System;

namespace Framework.Core
{
    public class TargetEntityLogicalNameAttribute : Attribute
    {
        public string TargetEntityLogicalName { get; }

        public TargetEntityLogicalNameAttribute(string targetEntityLogicalName)
        {
            TargetEntityLogicalName = targetEntityLogicalName;
        }
    }
}
