using System;
using System.Collections.Generic;
using Bogus;
using LightOps.Commerce.Services.MetaField.Api.Models;
using LightOps.Commerce.Services.MetaField.Domain.Models;

namespace Sample.MetaFieldService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }
        
        public int MetaFieldsPerEntity { get; set; } = 1;

        public IList<IMetaField> MetaFields { get; internal set; } = new List<IMetaField>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            for (var i = 0; i <= 30; i++)
            {
                // Add meta-fields to content pages
                GetMetaFieldFaker("ContentPage", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                // Add meta-fields to navigations
                GetMetaFieldFaker("Navigation", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                // Add meta-fields to categories
                GetMetaFieldFaker("Category", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                // Add meta-fields to products
                GetMetaFieldFaker("Product", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                GetMetaFieldFaker("ProductVariant", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
            }
        }

        private Faker<MetaField> GetMetaFieldFaker(string parentEntity, string parentId)
        {
            return new Faker<MetaField>()
                .RuleFor(x => x.Id, f => $"gid://MetaField/{f.UniqueIndex}")
                .RuleFor(x => x.ParentId, f => $"gid://{parentEntity}/{parentId}")
                .RuleFor(x => x.Namespace, f => "core")
                .RuleFor(x => x.Name, f => "material")
                .RuleFor(x => x.Type, f => "text")
                .RuleFor(x => x.Value, f => f.Lorem.Sentence())
                .RuleFor(x => x.CreatedAt, f => f.Date.Past(2))
                .RuleFor(x => x.UpdatedAt, f => f.Date.Past());
        }
    }
}