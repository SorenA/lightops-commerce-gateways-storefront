using System;
using System.Collections.Generic;
using Bogus;
using LightOps.Commerce.Services.MetaField.Api.Models;
using LightOps.Commerce.Services.MetaField.Domain.Models;

namespace Sample.MetaFieldService.Data
{
    public class BogusMetaFieldFactory
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

            for (var i = 0; i <= 3; i++)
            {
                // Add meta-fields to content page
                GetMetaFieldFaker("content_page", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
                // Add meta-fields to navigation
                GetMetaFieldFaker("navigation", i.ToString()).Generate(MetaFieldsPerEntity).ForEach(MetaFields.Add);
            }
        }

        private Faker<MetaField> GetMetaFieldFaker(string parentEntityType, string parentEntityId)
        {
            return new Faker<MetaField>()
                .RuleFor(x => x.ParentEntityType, f => parentEntityType)
                .RuleFor(x => x.ParentEntityId, f => parentEntityId)
                .RuleFor(x => x.Name, f => "material")
                .RuleFor(x => x.Type, f => "text")
                .RuleFor(x => x.Value, f => f.Lorem.Sentence());
        }
    }
}