using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using LightOps.Commerce.Services.Category.Api.Models;
using LightOps.Commerce.Services.Category.Domain.Models;

namespace Sample.CategoryService.Data
{
    public class BogusCategoryFactory
    {
        public int? Seed { get; set; }

        public int RootCategories { get; set; } = 2;
        public int CategoriesPerRootCategory { get; set; } = 3;

        public IList<ICategory> Categories { get; internal set; } = new List<ICategory>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            ICategory firstLeafCategory = null;

            // Add root categories
            var rootCategories = GetCategoryFaker().Generate(RootCategories);
            foreach (var rootCategory in rootCategories)
            {
                Categories.Add(rootCategory);

                // Add leaf categories
                var leafCategories = GetCategoryFaker(rootCategory.Id).Generate(CategoriesPerRootCategory);
                foreach (var leafCategory in leafCategories)
                {
                    if (firstLeafCategory == null)
                    {
                        firstLeafCategory = leafCategory;
                    }

                    Categories.Add(leafCategory);
                }
            }
        }

        private Faker<Category> GetCategoryFaker(string parentCategoryId = null)
        {
            return new Faker<Category>()
                .RuleFor(x => x.Id, f => f.UniqueIndex.ToString())
                .RuleFor(x => x.Handle, (f, x) => $"category-{x.Id}")
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath())
                .RuleFor(x => x.Title, f => f.Commerce.Categories(1).First())
                .RuleFor(x => x.Description, (f, x) => $"{x.Title} - Description")
                .RuleFor(x => x.SeoTitle, (f, x) => $"{x.Title}")
                .RuleFor(x => x.SeoDescription, (f, x) => $"{x.Description}")
                .RuleFor(x => x.ParentCategoryId, f => parentCategoryId)
                .RuleFor(x => x.PrimaryImage, f => f.Image.PicsumUrl());
        }
    }
}
