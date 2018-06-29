using System;
using System.Collections.Generic;

namespace Qubit.Xrm.Framework.Mock.Core
{
    public class MockOptionsResourceBase
    {
        public string PrimaryEntityName { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public string Store { get; set; }

        public IEnumerable<MockParameter> InputParameters { get; set; }
    }
}
