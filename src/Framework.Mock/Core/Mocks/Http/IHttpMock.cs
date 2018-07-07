using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Matchers.Request;
using WireMock.ResponseProviders;
using WireMock.Server;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Http
{
    public interface IHttpMock
    {
        FluentMockServer Server { get; }
        void SetupRequest(IRequestMatcher requestMatcher, IResponseProvider responseProvider);
        void Shutdown();
    }
}
