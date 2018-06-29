using System;
using System.Collections.Generic;
using System.Linq;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Ninject;
using Qubit.Xrm.Framework.Mock.Core.Mocks.Services.Logging;
using Qubit.Xrm.Framework.Mock.Core.Mocks.Services.MockWeb;
using Qubit.Xrm.Framework.Mock.Core.MockStore;
using Seterlund.CodeGuard;

namespace Qubit.Xrm.Framework.Mock.Core
{
    public abstract class AbstractMock<TImplementation, TTestFixture, TMockOptions, TOptionsResource, TExecutionContext> : IMock
        where TTestFixture : new()
        where TMockOptions : AbstractMockOptions<TTestFixture, TOptionsResource>
        where TOptionsResource : MockOptionsResourceBase
        where TExecutionContext : class, IExecutionContext, new()
    {
        protected IKernel FakedServices { get; private set; }
        protected TImplementation ImplementationInstance { get; private set; }
        protected TMockOptions MockOptions { get; }
        protected XrmFakedContext FakedContext { get; private set; }
        protected TExecutionContext FakedExecutionContext { get; private set; }

        protected AbstractMock(Func<TMockOptions> optionsBuilder)
        {
            MockOptions = optionsBuilder();
            SetupMock();
        }

        private void AddFakeData(Entity fakeEntity)
        {
            if (FakedContext.Data.Any(e => e.Key == fakeEntity.LogicalName))
            {
                FakedContext.Data[fakeEntity.LogicalName].Add(fakeEntity.Id, fakeEntity);
            }
            else
            {
                FakedContext.Data.Add(fakeEntity.LogicalName, new Dictionary<Guid, Entity>());
                AddFakeData(fakeEntity);
            }
        }

        private void SetupMock()
        {
            FakedServices = MockOptions.FakedServices;

            FakedServices.AddFakeLogging()
                .AddFakeHttpHandler();

            ImplementationInstance = (TImplementation)Activator.CreateInstance(typeof(TImplementation), FakedServices);

            //Setup Facked Context
            FakedContext = new XrmFakedContext
            {
                ProxyTypesAssembly = MockOptions.ProxyTypesAssembly ?? typeof(TImplementation).Assembly
            };

            //Build system store
            foreach (Entity entity in MockOptions.Store)
            {
                AddFakeData(entity);
            }

            foreach (MockRelationship relationship in MockOptions.Relationships)
            {
                FakedContext.AddRelationship(relationship.RelationshipName, (XrmFakedRelationship)relationship);
            }

            var fakedService = FakedContext.GetFakedOrganizationService();
            foreach (AssociateRequest associationRequest in MockOptions.AssociateRequests)
            {
                fakedService.Execute(associationRequest);
            }

            if (typeof(TExecutionContext) == typeof(XrmFakedPluginExecutionContext))
            {
                XrmFakedPluginExecutionContext executionContext = FakedContext.GetDefaultPluginContext();
                executionContext.InputParameters = MockOptions.InputParameters;
                executionContext.CorrelationId = Guid.NewGuid();
                executionContext.OperationId = Guid.NewGuid();

                Guard.That(() => MockOptions.Message).IsNotNullOrEmpty();
                executionContext.MessageName = MockOptions.Message;

                Guard.That(() => MockOptions.UserId).IsNotEqual(Guid.Empty);
                executionContext.UserId = MockOptions.UserId;

                executionContext.PrimaryEntityName = MockOptions.PrimaryEntityName;

                FakedExecutionContext = executionContext as TExecutionContext;
            }
            else if (typeof(TExecutionContext) == typeof(XrmFakedWorkflowContext))
            {
                XrmFakedWorkflowContext executionContext = FakedContext.GetDefaultWorkflowContext();
                executionContext.InputParameters = MockOptions.InputParameters;
                executionContext.CorrelationId = Guid.NewGuid();
                executionContext.OperationId = Guid.NewGuid();

                Guard.That(() => MockOptions.Message).IsNotNullOrEmpty();
                executionContext.MessageName = MockOptions.Message;

                Guard.That(() => MockOptions.UserId).IsNotEqual(Guid.Empty);
                executionContext.UserId = MockOptions.UserId;

                executionContext.PrimaryEntityName = MockOptions.PrimaryEntityName;

                FakedExecutionContext = executionContext as TExecutionContext;
            }

            Guard.That(() => FakedExecutionContext).IsNotNull();

            Setup();
        }

        protected abstract void Setup();

        public abstract void Test(Action<IXrmContext, IKernel> testCriteria);

        public virtual void Dispose()
        { }

    }
}
