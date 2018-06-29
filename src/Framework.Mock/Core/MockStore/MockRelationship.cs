using FakeXrmEasy;

namespace Qubit.Xrm.Framework.Mock.Core.MockStore
{
    public class MockRelationship
    { 
        public string RelationshipName { get; set; }
        public string IntersectEntity { get; set; }
        public string Entity1LogicalName { get; set; }
        public string Entity1Attribute { get; set; }
        public string Entity2LogicalName { get; set; }
        public string Entity2Attribute { get; set; }

        public static explicit operator XrmFakedRelationship(MockRelationship toConvert)
        {
            return new XrmFakedRelationship
            {
                IntersectEntity = toConvert.IntersectEntity,
                Entity1LogicalName = toConvert.Entity1LogicalName,
                Entity1Attribute = toConvert.Entity1Attribute,
                Entity2LogicalName = toConvert.Entity2LogicalName,
                Entity2Attribute = toConvert.Entity2Attribute
            };
        }
    }
}
