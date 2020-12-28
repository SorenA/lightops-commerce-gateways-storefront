using System;
using System.Collections.Generic;
using Bogus;
using LightOps.Commerce.Services.Category.Api.Models;
using LightOps.Commerce.Services.Category.Domain.Models;

namespace Sample.CategoryService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }

        public int RootEntities { get; set; } = 5;
        public int LeafEntities { get; set; } = 10;

        public IList<ICategory> Categories { get; internal set; } = new List<ICategory>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            // Add root entities
            var rootEntities = GetCategoryFaker().Generate(RootEntities);
            foreach (var rootEntity in rootEntities)
            {
                Categories.Add(rootEntity);

                // Add leaf entities
                var leafEntities = GetCategoryFaker(rootEntity.Id).Generate(LeafEntities);
                foreach (var leafEntity in leafEntities)
                {
                    Categories.Add(leafEntity);
                }
            }
        }

        private Faker<Category> GetCategoryFaker(string parentId = null)
        {
            return new Faker<Category>()
                .RuleFor(x => x.Id, f => $"gid://Category/{f.UniqueIndex}")
                .RuleFor(x => x.ParentId, f => parentId ?? "gid://")
                .RuleFor(x => x.Handle, (f, x) => $"category-{f.UniqueIndex}")
                .RuleFor(x => x.Title, f => f.Address.City())
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath())
                .RuleFor(x => x.Type, f => "category")
                .RuleFor(x => x.Description, (f, x) => $"{x.Title} - Description")
                .RuleFor(x => x.CreatedAt, f => f.Date.Past(2))
                .RuleFor(x => x.UpdatedAt, f => f.Date.Past())
                .RuleFor(x => x.PrimaryImage, f => new Image
                {
                    Id = $"gid://Image/1000{f.UniqueIndex}",
                    Url = f.Image.PicsumUrl(),
                    AltText = f.Lorem.Sentence(),
                    FocalCenterTop = f.Random.Double(0, 1),
                    FocalCenterLeft = f.Random.Double(0, 1),
                });
        }
    }
}
