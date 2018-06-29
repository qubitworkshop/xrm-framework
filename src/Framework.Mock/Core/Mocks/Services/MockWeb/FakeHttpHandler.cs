using System.Collections.Generic;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Services.MockWeb
{
    public class FakeHttpHandler : IFakeHttpHandler
    {
        public Dictionary<string, Dictionary<string, object>> Requests { get; }

        public FakeHttpHandler()
        {
            Requests = new Dictionary<string, Dictionary<string, object>>();
        }

        private Dictionary<string, object> EnsureMethod(string method)
        {
            if(!Requests.ContainsKey(method))
            {
                Requests.Add(method, new Dictionary<string, object>());
            }

            return Requests[method];
        }

        public void PostBody(string action, object body)
        {
            EnsureMethod("POST").Add(action, body);
        }

        public void Get(string action)
        {
            EnsureMethod("GET").Add(action, new { });
        }
    }
}
