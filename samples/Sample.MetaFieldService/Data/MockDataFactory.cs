using System;
using System.Collections.Generic;
using Bogus;
using Google.Protobuf.WellKnownTypes;
using LightOps.Commerce.Proto.Types;

namespace Sample.MetaFieldService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }
        
        public int MetaFieldsPerEntity { get; set; } = 1;

        public IList<MetaField> MetaFields { get; internal set; } = new List<MetaField>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            for (var i = 0; i <= 30; i++)
            {
                // Add meta-fields to content pages
                GetMetaFieldFaker("ContentPage", i.ToString(), true).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                // Add meta-fields to navigations
                GetMetaFieldFaker("Navigation", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                // Add meta-fields to categories
                GetMetaFieldFaker("Category", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                // Add meta-fields to products
                GetMetaFieldFaker("Product", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                GetMetaFieldFaker("ProductVariant", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
            }
        }

        private Faker<MetaField> GetMetaFieldFaker(string parentEntity, string parentId, bool isLocalized = false)
        {
            return new Faker<MetaField>()
                .RuleFor(x => x.Id, f => $"gid://MetaField/{f.UniqueIndex}")
                .RuleFor(x => x.ParentId, f => $"gid://{parentEntity}/{parentId}")
                .RuleFor(x => x.Namespace, f => "core")
                .RuleFor(x => x.Name, f => "material")
                .RuleFor(x => x.Type, f => "text")
                .RuleFor(x => x.Value, f => f.Lorem.Sentence())
                .RuleFor(x => x.CreatedAt, f => Timestamp.FromDateTime(f.Date.Past(2).ToUniversalTime()))
                .RuleFor(x => x.UpdatedAt, f => Timestamp.FromDateTime(f.Date.Past().ToUniversalTime()))
                .RuleFor(x => x.IsPublic, f => f.Random.Bool())
                .FinishWith((f, x) =>
                {
                    if (isLocalized)
                    {
                        x.LocalizedValues = new MetaField.Types.LocalizedStringList
                        {
                            Values = {GetLocalizedStrings(f.Lorem.Sentence())},
                        };
                    }
                });
        }

        private IList<LocalizedString> GetLocalizedStrings(string value, bool isUrl = false)
        {
            return new List<LocalizedString>
            {
                new LocalizedString
                {
                    LanguageCode = "en-US",
                    Value = isUrl
                        ? $"/en-us{value}"
                        : $"{value} [en-US]",
                },
                new LocalizedString
                {
                    LanguageCode = "da-DK",
                    Value = isUrl
                        ? $"/da-dk{value}"
                        : $"{value} [da-DK]",
                }
            };
        }
    }
}