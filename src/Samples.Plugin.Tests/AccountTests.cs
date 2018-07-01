using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Qubit.Xrm.Framework.Abstractions.Configuration;
using Qubit.Xrm.Framework.Mock.Core;
using Qubit.Xrm.Framework.Mock.Plugins;
using Samples.Plugin;

namespace Plugins.Tests
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void When_AccountCreated_Expect_AccountAutonumberToBePopulated()
        {
            var testOptions = new MockOptions<AccountTests>("Samples.Plugin.Tests.Mocks.TestCases.Account.Create.AutoNumber.context.json");

            IMock pluginMock = new PluginMock<SampleEntityPipelinePlugin, AccountTests, DefaultSettingsProvider>(() => testOptions);
            pluginMock.Test((resultContext, services) =>
            {

            });
        }
    }
}
