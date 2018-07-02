using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Ninject;
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
                IOrganizationService organizationService = services.Get<IOrganizationService>();
                Entity accountEntity = organizationService.Retrieve("account", new Guid("A4766A71-1D2E-4374-8B6D-354476D1C1EC"), new ColumnSet(true));

                Assert.IsTrue(accountEntity.Contains("qubit_autonumber"));
                Assert.IsTrue(accountEntity.GetAttributeValue<string>("qubit_autonumber") == "12345");
            });
        }

        [Test]
        public void When_AccountCreated_Expect_CreatedBy()
        {
            var testOptions = new MockOptions<AccountTests>("Samples.Plugin.Tests.Mocks.TestCases.Account.Create.Relationships.context.json");

            IMock pluginMock = new PluginMock<SampleEntityPipelinePlugin, AccountTests, DefaultSettingsProvider>(() => testOptions);
            pluginMock.Test((resultContext, services) =>
            {
                IOrganizationService organizationService = services.Get<IOrganizationService>();
                Entity accountEntity = organizationService.Retrieve("account", new Guid("A4766A71-1D2E-4374-8B6D-354476D1C1EC"), new ColumnSet(true));

                Assert.IsTrue(accountEntity.Contains("createdby"));
                Assert.IsInstanceOf(typeof(EntityReference), accountEntity["createdby"]);
                Assert.IsTrue(accountEntity.GetAttributeValue<EntityReference>("createdby").Id.Equals(new Guid("419308E5-001F-4896-8AD2-ABBB76E2E66B")));
            });
        }
    }
}
