using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json.Linq;

namespace Qubit.Xrm.Framework.Mock.Core.MockStore
{
    internal class MockEntity : MockEntityReference
    {
        public IEnumerable<JObject> Attributes { get; set; }

        public static explicit operator Entity(MockEntity toConvert)
        {
            Entity entity = new Entity(toConvert.LogicalName, toConvert.Id);

            //Attributes
            foreach (dynamic attributeObject in toConvert.Attributes)
            {
                string name = attributeObject.Name;
                string type = attributeObject.Type;

                switch (type.ToUpper())
                {
                    case "OPTIONSETVALUE":
                        var optionSetValue = new OptionSetValue(attributeObject.Value.ToObject<int>());
                        entity.Attributes.Add(name, optionSetValue);
                        break;
                    case "ENTITYREFERENCE":
                        var entityReference = new EntityReference(attributeObject.EntityLogicalName.ToObject<string>(), attributeObject.Value.ToObject<Guid>());
                        entity.Attributes.Add(name, entityReference);
                        break;
                    case "STRING":
                        entity.Attributes.Add(name, attributeObject.Value.ToObject<string>());
                        break;
                    case "BOOL":
                        entity.Attributes.Add(name, attributeObject.Value.ToObject<bool>());
                        break;
                    case "INT":
                        entity.Attributes.Add(name, attributeObject.Value.ToObject<int>());
                        break;
                    case "DOUBLE":
                        entity.Attributes.Add(name, attributeObject.Value.ToObject<double>());
                        break;
                    case "DATETIME":
                        entity.Attributes.Add(name, attributeObject.Value.ToObject<DateTime>());
                        break;
                    case "DATETIME?":
                        entity.Attributes.Add(name, attributeObject.Value.ToObject<DateTime?>());
                        break;
                    case "DECIMAL":
                        entity.Attributes.Add(name, attributeObject.Value.ToObject<decimal>());
                        break;
                    case "MONEY":
                        entity.Attributes.Add(name, new Money(attributeObject.Value.ToObject<decimal>()));
                        break;
                }
                
            }

            return entity;
        }
    }
}
