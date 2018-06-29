using System;
using FakeXrmEasy;
using Ninject;

namespace Qubit.Xrm.Framework.Mock.Core
{
    public interface IMock
    {
        void Test(Action<IXrmContext, IKernel> testCriteria);
    }
}
