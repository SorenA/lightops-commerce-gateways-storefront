using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class MetaFieldGraphType : ObjectGraphType<IMetaField>
    {
        public MetaFieldGraphType()
        {
            Name = "MetaField";

            Field(m => m.Name);
            Field(m => m.Type);
            Field(m => m.Value);
        }
    }
}