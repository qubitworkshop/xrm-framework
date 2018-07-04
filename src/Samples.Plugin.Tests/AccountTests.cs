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
using Qubit.Xrm.Framework.Mock.Core.Mocks.Http;
using Qubit.Xrm.Framework.Mock.Plugins;
using Samples.Plugin;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Settings;

namespace Plugins.Tests
{
    [TestFixture]
    public class AccountTests
    {
        private void SetupMockWebServices(IKernel services)
        {
            ISettingsProvider settingsProvider = services.Get<ISettingsProvider>();
            string abnUrl = settingsProvider.Get("abnservice");

            IFluentMockServerSettings mockServerSettings = new FluentMockServerSettings
            {
                Urls = new[]
                {
                    abnUrl
                }
            };

            services.Bind<IFluentMockServerSettings>()
                .ToConstant(mockServerSettings)
                .InTransientScope();

            IHttpMock httpMock = services.Get<IHttpMock>();
            httpMock.SetupRequest(Request.Create().WithPath("/validate").UsingGet(), Response.Create().WithBody("Test"));
        }

        /// <summary>
        /// Basic test to get the plugin to execute and test core functionality
        /// </summary>
        [Test]
        public void When_AccountCreated_Expect_AccountAutonumberToBePopulated()
        {
            var testOptions = new MockOptions<AccountTests>("Samples.Plugin.Tests.Mocks.TestCases.Account.Create.AutoNumber.context.json");

            IMock pluginMock = new PluginMock<SampleEntityPipelinePlugin, AccountTests, DefaultSettingsProvider>(() => testOptions)
            {
                SetupMockServices = SetupMockWebServices
            };

            pluginMock.Test((resultContext, services) =>
            {
                IOrganizationService organizationService = services.Get<IOrganizationService>();
                Entity accountEntity = organizationService.Retrieve("account", new Guid("A4766A71-1D2E-4374-8B6D-354476D1C1EC"), new ColumnSet(true));

                Assert.IsTrue(accountEntity.Contains("qubit_autonumber"));
                Assert.IsTrue(accountEntity.GetAttributeValue<string>("qubit_autonumber") == "12345");
            });
        }


        /// <summary>
        /// Testing 1:N relationships with plugin
        /// </summary>
        [Test]
        public void When_AccountCreated_Expect_CreatedBy()
        {
            var testOptions = new MockOptions<AccountTests>("Samples.Plugin.Tests.Mocks.TestCases.Account.Create.Relationships.context.json");

            IMock pluginMock = new PluginMock<SampleEntityPipelinePlugin, AccountTests, DefaultSettingsProvider>(() => testOptions)
            {
                SetupMockServices = SetupMockWebServices
            };

            pluginMock.Test((resultContext, services) =>
            {
                IOrganizationService organizationService = services.Get<IOrganizationService>();
                Entity accountEntity = organizationService.Retrieve("account", new Guid("A4766A71-1D2E-4374-8B6D-354476D1C1EC"), new ColumnSet(true));

                Assert.IsTrue(accountEntity.Contains("createdby"));
                Assert.IsInstanceOf(typeof(EntityReference), accountEntity["createdby"]);

                EntityReference createdByEntityReference = accountEntity.GetAttributeValue<EntityReference>("createdby");
                Assert.IsTrue(createdByEntityReference.Id.Equals(new Guid("419308E5-001F-4896-8AD2-ABBB76E2E66B")));

                Entity createEntity = organizationService.Retrieve(createdByEntityReference.LogicalName, createdByEntityReference.Id, new ColumnSet(true));
                Assert.IsTrue(createEntity.Contains("domainname"));
                Assert.IsTrue(createEntity.GetAttributeValue<string>("domainname") == "DEVELOPMENT\\user1");
            });
        }

        [Test]
        public void When_AccountCreated_Expect_AbnObtainedFromWebUrl()
        {
            var testOptions = new MockOptions<AccountTests>("Samples.Plugin.Tests.Mocks.TestCases.Account.Create.Relationships.context.json");

            IMock pluginMock = new PluginMock<SampleEntityPipelinePlugin, AccountTests, DefaultSettingsProvider>(() => testOptions)
            {
                SetupMockServices = SetupMockWebServices
            };

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
