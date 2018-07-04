using System;
using FakeXrmEasy;
using Ninject;
using NUnit.Framework;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Core;
using Qubit.Xrm.Framework.Mock.Core;
using Qubit.Xrm.Framework.Mock.Core.Mocks.Http;

namespace Qubit.Xrm.Framework.Mock.Plugins
{
    public class PluginMock<TPlugin, TTestFixture, TSettingsProvider> 
        : AbstractMock<TTestFixture, MockOptions<TTestFixture>, MockOptionsResource, XrmFakedPluginExecutionContext>
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
            TPlugin pluginInstance = (TPlugin)Activator.CreateInstance(typeof(TPlugin), FakedServices, SetupMockServices);
            Assert.DoesNotThrow(() => FakedContext.ExecutePluginWith(FakedExecutionContext, pluginInstance));
            testCriteria(FakedContext, FakedServices);
            FakedServices.RemoveHttpMock();
        }
    }

    public class PluginMock<TPlugin, TTestFixture> : PluginMock<TPlugin, TTestFixture, DefaultSettingsProvider>
        where TPlugin : Plugin<DefaultSettingsProvider>, new()
        where TTestFixture : new()
    {
        public PluginMock(Func<MockOptions<TTestFixture>> optionsBuilder) : base(optionsBuilder)
        { }
    }
}
