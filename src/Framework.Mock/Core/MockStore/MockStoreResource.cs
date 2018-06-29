using System.Collections.Generic;

namespace Qubit.Xrm.Framework.Mock.Core.MockStore
{
    internal class MockStoreResource
    {
        public IEnumerable<string> Inherits { get; set; }
        public IEnumerable<MockEntity> MockEntities { get; set; }
        public IEnumerable<MockRelationship> MockRelationships { get; set; }
        public IEnumerable<MockAssociation> MockAssociations { get; set; }
    }
}
