using System;
using Microsoft.Xrm.Sdk;

namespace Qubit.Xrm.Framework.Mock.Core.MockStore
{
    public class MockEntityReference
    {
        public Guid Id { get; set; }
        public string LogicalName { get; set; }

        public static explicit operator EntityReference(MockEntityReference toConvert)
        {
            EntityReference entityReference = new EntityReference(toConvert.LogicalName, toConvert.Id);
            return entityReference;
        }
    }
}
