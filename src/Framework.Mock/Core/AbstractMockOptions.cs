using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Newtonsoft.Json.Linq;
using Ninject;
using Qubit.Xrm.Framework.Mock.Core.MockStore;

namespace Qubit.Xrm.Framework.Mock.Core
{
    public abstract class AbstractMockOptions<TTestFixture, TOptionsResource> 
        where TTestFixture : new()
        where TOptionsResource : MockOptionsResourceBase
    {
        private readonly Assembly _testFixtureAssembly;
        private readonly string _mockResourceName;

        protected TOptionsResource MockOptionsResource { get; private set; }

        protected AbstractMockOptions(string mockResourceName)
        {
            _mockResourceName = mockResourceName;
            _testFixtureAssembly = typeof(TTestFixture).Assembly;

            FakedServices = new StandardKernel();

            Initialize();
        }

        public IKernel FakedServices { get; }

        public string PrimaryEntityName { get; private set; }
        public string Message { get; private set; }
        public Guid UserId { get; set; }

        public ParameterCollection InputParameters { get; set; }

        public Assembly ProxyTypesAssembly { get; set; }

        public List<MockRelationship> Relationships { get; set; }
        public List<Entity> Store { get; set; }
        public List<AssociateRequest> AssociateRequests { get; set; }

        private void Initialize()
        {
            JToken mockOptionsResourceObject = JToken.Parse(_testFixtureAssembly.ReadEmbeddedResourceAsString(_mockResourceName));
            MockOptionsResource = mockOptionsResourceObject.ToObject<TOptionsResource>();

            Store = new List<Entity>();
            Relationships = new List<MockRelationship>();
            AssociateRequests = new List<AssociateRequest>();

            PrimaryEntityName = MockOptionsResource.PrimaryEntityName;
            Message = MockOptionsResource.Message;
            UserId = MockOptionsResource.UserId;

            InputParameters = new ParameterCollection();
            foreach (MockParameter inputParameter in MockOptionsResource.InputParameters)
            {
                object parameterValue = null;

                if (inputParameter.Value is JObject inputeParameterValueJObject)
                {
                    switch (inputParameter.Type)
                    {
                        case "Entity":
                            parameterValue = (Entity)inputeParameterValueJObject.ToObject<MockEntity>();
                            break;
                        case "EntityReference":
                            parameterValue = (EntityReference)inputeParameterValueJObject.ToObject<MockEntityReference>();
                            break;
                        default:
                            parameterValue = inputeParameterValueJObject.ToObject<object>();
                            break;
                    }
                }
                else
                {
                    switch (inputParameter.Type)
                    {
                        case "Guid":
                            parameterValue = Guid.Parse(inputParameter.Value.ToString());
                            break;
                        default:
                            parameterValue = inputParameter.Value;
                            break;
                    }
                }

                InputParameters.Add(inputParameter.Name, parameterValue);
            }

            LoadMockStore(MockOptionsResource.Store);

            LoadMockResource();
        }

        private void LoadMockStore(string mockStoreResourceName)
        {
            MockStoreResource mockStoreResource = _testFixtureAssembly.GetEmbeddedJsonResource<MockStoreResource>(mockStoreResourceName);

            if (mockStoreResource.MockEntities != null)
            {
                Store.AddRange(mockStoreResource.MockEntities.Select(e => (Entity)e));
            }

            if (mockStoreResource.MockRelationships != null)
            {
                Relationships.AddRange(mockStoreResource.MockRelationships);
            }

            if (mockStoreResource.MockAssociations != null)
            {
                AssociateRequests.AddRange(mockStoreResource.MockAssociations.Select(e => (AssociateRequest)e));
            }

            if (mockStoreResource.Inherits != null)
            {
                foreach (string inherittedStore in mockStoreResource.Inherits)
                {
                    LoadMockStore(inherittedStore);
                }
            }
        }

        protected abstract void LoadMockResource();
    }
}
