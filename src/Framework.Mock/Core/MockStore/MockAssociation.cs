using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Qubit.Xrm.Framework.Mock.Core.MockStore
{
    public class MockAssociation
    {
        public string RelationshipName { get; set; }
        public MockEntityReference TargetEntity { get; set; }
        public IEnumerable<MockEntityReference> RelatedEntities { get; set; }

        public static explicit operator AssociateRequest(MockAssociation toConvert)
        {
            EntityReferenceCollection toEntityReferences = new EntityReferenceCollection();
            toEntityReferences.AddRange(toConvert.RelatedEntities.Select(e => (EntityReference)e));

            return new AssociateRequest()
            {
                Target = (EntityReference)toConvert.TargetEntity,
                RelatedEntities = toEntityReferences,
                Relationship = new Relationship(toConvert.RelationshipName)
            };
        }
    }
}
