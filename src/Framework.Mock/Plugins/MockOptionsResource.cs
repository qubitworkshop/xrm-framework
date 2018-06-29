using Qubit.Xrm.Framework.Mock.Core;

namespace Qubit.Xrm.Framework.Mock.Plugins
{
    public class MockOptionsResource : MockOptionsResourceBase
    {
        public int Stage { get; set; }
        public string UnSecureConfiguration { get; set; }
        public string SecureConfiguration { get; set; }
    }
}
