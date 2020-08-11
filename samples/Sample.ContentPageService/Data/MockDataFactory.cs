using System;
using System.Collections.Generic;
using Bogus;
using LightOps.Commerce.Services.ContentPage.Api.Models;
using LightOps.Commerce.Services.ContentPage.Domain.Models;

namespace Sample.ContentPageService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }

        public int RootEntities { get; set; } = 2;
        public int LeafEntities { get; set; } = 3;

        public IList<IContentPage> ContentPages { get; internal set; } = new List<IContentPage>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            // Add root entities
            var rootEntities = GetContentPageFaker().Generate(RootEntities);
            foreach (var rootEntity in rootEntities)
            {
                ContentPages.Add(rootEntity);

                // Add leaf entities
                var leafEntities = GetContentPageFaker(rootEntity.Id).Generate(LeafEntities);
                foreach (var leafEntity in leafEntities)
                {
                    ContentPages.Add(leafEntity);
                }
            }
        }

        private Faker<ContentPage> GetContentPageFaker(string parentId = null)
        {
            return new Faker<ContentPage>()
                .RuleFor(x => x.Id, f => $"gid://ContentPage/{f.UniqueIndex}")
                .RuleFor(x => x.ParentId, f => parentId ?? "gid://")
                .RuleFor(x => x.Handle, (f, x) => $"content-page-{f.UniqueIndex}")
                .RuleFor(x => x.Title, f => f.Address.City())
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath())
                .RuleFor(x => x.Type, f => "page")
                .RuleFor(x => x.Summary, (f, x) => $"{x.Title} - Summary")
                .RuleFor(x => x.CreatedAt, f => f.Date.Past(2))
                .RuleFor(x => x.UpdatedAt, f => f.Date.Past())
                .RuleFor(x => x.PrimaryImage, f => new Image
                {
                    Id = $"gid://Image/1000{f.UniqueIndex}",
                    Url = f.Image.PicsumUrl(),
                    AltText = f.Lorem.Sentence(),
                });
        }
    }
}
