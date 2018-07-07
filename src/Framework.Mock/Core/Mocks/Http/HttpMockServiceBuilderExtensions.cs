using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Http
{
    public static class HttpMockServiceBuilderExtensions
    {
        public static IKernel AddHttpMock(this IKernel services)
        {
            Guard.That(() => services).IsNotNull();

            //Service Bus services
            services.Bind<IHttpMock>().To<HttpMock>().InSingletonScope();

            return services;
        }

        public static IKernel RemoveHttpMock(this IKernel services)
        {
            Guard.That(() => services).IsNotNull();
            IHttpMock httpMock = services.TryGet<IHttpMock>();

            if (httpMock != null)
            {
                httpMock.Shutdown();

                //Service Bus services
                services.RemoveBinding(services.GetBindings(typeof(IHttpMock)).First());
            }

            return services;
        }
    }
}
