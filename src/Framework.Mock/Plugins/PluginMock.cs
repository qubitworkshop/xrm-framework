using System;
using FakeXrmEasy;
using Ninject;
using NUnit.Framework;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Core;
using Qubit.Xrm.Framework.Mock.Core;

namespace Qubit.Xrm.Framework.Mock.Plugins
{
    public class PluginMock<TPlugin, TTestFixture, TSettingsProvider> 
        : AbstractMock<TPlugin, TTestFixture, MockOptions<TTestFixture>, MockOptionsResource, XrmFakedPluginExecutionContext>
        where TPlugin : Plugin<TSettingsProvider>, new()
        where TTestFixture: new()
        where TSettingsProvider : ISettingsProvider
    {
        protected override void Setup()
        {
            //Setup plugin execution context
            FakedExecutionContext.OutputParameters = MockOptions.OutputParameters;
            FakedExecutionContext.PreEntityImages = MockOptions.PreEntityImages;
            FakedExecutionContext.PostEntityImages = MockOptions.PostEntityImages;
            FakedExecutionContext.Stage = MockOptions.Stage;
        }

        public PluginMock(Func<MockOptions<TTestFixture>> optionsBuilder) : base(optionsBuilder)
        { }

        public override void Test(Action<IXrmContext, IKernel> testCriteria)
        {
            Assert.DoesNotThrow(() => FakedContext.ExecutePluginWith(FakedExecutionContext, ImplementationInstance));
            testCriteria(FakedContext, FakedServices);
        }
    }
}
