using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Qubit.Xrm.Framework.Helpers
{
    public static class MetadataExtensions
    {
        public static string ToLabel(this OptionSetValue opetionSetValue, string entityName, string fieldName, IOrganizationService service)
        {

            var attReq = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = fieldName,
                RetrieveAsIfPublished = true
            };

            var attResponse = (RetrieveAttributeResponse)service.Execute(attReq);
            var attMetadata = (EnumAttributeMetadata)attResponse.AttributeMetadata;

            return attMetadata.OptionSet.Options.FirstOrDefault(x => x.Value == opetionSetValue.Value)?.Label.UserLocalizedLabel.Label;

        }
    }
}
