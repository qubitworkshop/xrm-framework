using System;

namespace Qubit.Xrm.Framework.Core
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
