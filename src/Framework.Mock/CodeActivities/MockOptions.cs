using Qubit.Xrm.Framework.Mock.Core;

namespace Qubit.Xrm.Framework.Mock.CodeActivities
{
    public class MockOptions<TTestFixture> : AbstractMockOptions<TTestFixture, MockOptionsResource> where TTestFixture : new()
    {
        public MockOptions(string mockResourceName) : base(mockResourceName)
        { }

        protected override void LoadMockResource()
        {
            
        }
    }
}
