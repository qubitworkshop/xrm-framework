using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.Matchers.Request;
using WireMock.ResponseProviders;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Http
{
    public interface IHttpMock
    {
        void SetupRequest(IRequestMatcher requestMatcher, IResponseProvider responseProvider);
        void Shutdown();
    }
}
