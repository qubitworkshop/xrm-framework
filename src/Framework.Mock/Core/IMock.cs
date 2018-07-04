using System;
using FakeXrmEasy;
using Ninject;

namespace Qubit.Xrm.Framework.Mock.Core
{
    public interface IMock
    {
        Action<IKernel> SetupMockServices { get; set; }
        void Test(Action<IXrmContext, IKernel> testCriteria);
    }
}
