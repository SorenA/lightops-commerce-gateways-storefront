using System;
using GraphQL.Types;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class MetaFieldGraphType : ObjectGraphType<MetaField>
    {
        public MetaFieldGraphType()
        {
            Name = "MetaField";

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://MetaField/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("ParentId")
                .Description("Globally unique identifier of parent, 'gid://' if none")
                .Resolve(ctx => ctx.Source.ParentId);

            Field<StringGraphType, string>()
                .Name("Namespace")
                .Description("The namespace of the meta-field")
                .Resolve(ctx => ctx.Source.Namespace);

            Field<StringGraphType, string>()
                .Name("Name")
                .Description("The name of the meta-field")
                .Resolve(ctx => ctx.Source.Name);

            Field<StringGraphType, string>()
                .Name("Type")
                .Description("The type of the meta-field")
                .Resolve(ctx => ctx.Source.Type);

            Field<StringGraphType, string>()
                .Name("Value")
                .Description("The value of the meta-field")
                .Resolve(ctx => ctx.Source.Value);

            Field<DateTimeGraphType, DateTime>()
                .Name("CreatedAt")
                .Description("The timestamp of meta-field creation")
                .Resolve(ctx => ctx.Source.CreatedAt.ToDateTime());

            Field<DateTimeGraphType, DateTime>()
                .Name("UpdatedAt")
                .Description("The timestamp of the latest meta-field update")
                .Resolve(ctx => ctx.Source.UpdatedAt.ToDateTime());
        }
    }
}
