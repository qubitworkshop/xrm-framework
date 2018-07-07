using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Matchers.Request;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.ResponseProviders;
using WireMock.Server;
using WireMock.Settings;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Http
{
    public class HttpMock : IHttpMock
    {
        public HttpMock(IFluentMockServerSettings fluentMockServerSettings)
        {
            Server = FluentMockServer.Start(fluentMockServerSettings);
        }

        public FluentMockServer Server { get; }

        public void SetupRequest(IRequestMatcher requestMatcher, IResponseProvider responseProvider)
        {
            Server
                .Given(requestMatcher)
                .RespondWith(responseProvider);
        }

        public void Shutdown()
        {
            Server.Stop();
            Server.Dispose();
        }
    }
}
