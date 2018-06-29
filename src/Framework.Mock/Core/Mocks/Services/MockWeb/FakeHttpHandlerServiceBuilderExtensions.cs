using Ninject;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Mock.Core.Mocks.Services.MockWeb
{
    public static class FakeHttpHandlerServiceBuilderExtensions
    {
        public static IKernel AddFakeHttpHandler(this IKernel services)
        {
            Guard.That(() => services).IsNotNull();

            //Service Bus services
            services.Bind<IFakeHttpHandler>().To<FakeHttpHandler>().InSingletonScope();

            return services;
        }
    }
}
