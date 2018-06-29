using System;
using FakeXrmEasy;
using Ninject;
using NUnit.Framework;
using Qubit.Xrm.Framework.Mock.Core;

namespace Qubit.Xrm.Framework.Mock.CodeActivities
{
    public class CodeActivityMock<TCodeActivity, TTestFixture> 
        : AbstractMock<TCodeActivity, TTestFixture, MockOptions<TTestFixture>, MockOptionsResource, XrmFakedWorkflowContext>
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
            Assert.DoesNotThrow(() => FakedContext.ExecuteCodeActivity(FakedExecutionContext, null, ImplementationInstance));
            testCriteria(FakedContext, FakedServices);
        }
    }
}
