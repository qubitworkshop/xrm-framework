using System;
using FakeXrmEasy;
using Ninject;
using NUnit.Framework;
using Qubit.Xrm.Framework.Mock.Core;
using Qubit.Xrm.Framework.Mock.Core.Mocks.Http;

namespace Qubit.Xrm.Framework.Mock.CodeActivities
{
    public class CodeActivityMock<TCodeActivity, TTestFixture> 
        : AbstractMock<TTestFixture, MockOptions<TTestFixture>, MockOptionsResource, XrmFakedWorkflowContext>
        where TCodeActivity : WorkflowActivity, new()
        where TTestFixture : new()
    {
        public CodeActivityMock(Func<MockOptions<TTestFixture>> optionsBuilder) : base(optionsBuilder)
        {  }

        protected override void Setup()
        {

        }

        public override void Test(Action<IXrmContext, IKernel> testCriteria)
        {
            TCodeActivity codeActivityInstance = (TCodeActivity)Activator.CreateInstance(typeof(TCodeActivity), FakedServices, SetupMockServices);
            Assert.DoesNotThrow(() => FakedContext.ExecuteCodeActivity(FakedExecutionContext, null, codeActivityInstance));
            testCriteria(FakedContext, FakedServices);
            FakedServices.RemoveHttpMock();
        }
    }
}
