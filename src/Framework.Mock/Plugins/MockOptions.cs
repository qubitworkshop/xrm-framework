using Microsoft.Xrm.Sdk;
using Qubit.Xrm.Framework.Mock.Core;

namespace Qubit.Xrm.Framework.Mock.Plugins
{
    public class MockOptions<TTestFixture> : AbstractMockOptions<TTestFixture, MockOptionsResource> where TTestFixture : new()
    {
        protected override void LoadMockResource()
        {
            Stage = MockOptionsResource.Stage;
            UnSecureConfiguration = MockOptionsResource.UnSecureConfiguration;
            SecureConfiguration = MockOptionsResource.SecureConfiguration;
        }

        public MockOptions(string mockResourceName) : base(mockResourceName)
        { }

        public int Stage { get; set; }

        public string UnSecureConfiguration { get; set; }
        public string SecureConfiguration { get; set; }

        public ParameterCollection OutputParameters { get; set; }
        public EntityImageCollection PreEntityImages { get; set; }
        public EntityImageCollection PostEntityImages { get; set; }
    }
}
