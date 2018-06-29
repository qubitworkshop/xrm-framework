using System.Collections.Generic;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Services.MockWeb
{
    public interface IFakeHttpHandler
    {
        Dictionary<string, Dictionary<string, object>> Requests { get; }
        void PostBody(string action, object body);
        void Get(string action);
    }
}
