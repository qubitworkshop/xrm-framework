using System;

namespace Qubit.Xrm.Framework.Core
{
    /// <summary>
    /// Provides the framework with information on 
    /// which target entity to accept to execute the plugin against
    /// </summary>
    public class TargetEntityLogicalNameAttribute : Attribute
    {
        /// <summary>
        /// The logical name of the target entity
        /// </summary>
        public string TargetEntityLogicalName { get; }

        public TargetEntityLogicalNameAttribute(string targetEntityLogicalName)
        {
            TargetEntityLogicalName = targetEntityLogicalName;
        }
    }
}
