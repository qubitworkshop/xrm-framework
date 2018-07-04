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
        private readonly FluentMockServer _server;

        public HttpMock(IFluentMockServerSettings fluentMockServerSettings)
        {
            _server = FluentMockServer.Start(fluentMockServerSettings);
        }

        public void SetupRequest(IRequestMatcher requestMatcher, IResponseProvider responseProvider)
        {
            _server
                .Given(requestMatcher)
                .RespondWith(responseProvider);
        }

        public void Shutdown()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}
