using System;
using System.Collections.Generic;
using Bogus;
using LightOps.Commerce.Services.ContentPage.Api.Models;
using LightOps.Commerce.Services.ContentPage.Domain.Models;

namespace Sample.ContentPageService.Data
{
    public class BogusContentFactory
    {
        public int? Seed { get; set; }

        public int RootContentPages { get; set; } = 2;
        public int ContentPagesPerRootContentPages { get; set; } = 3;

        public IList<IContentPage> ContentPages { get; internal set; } = new List<IContentPage>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            // Add root content pages
            var rootContentPages = GetContentPageFaker().Generate(RootContentPages);
            foreach (var rootContentPage in rootContentPages)
            {
                ContentPages.Add(rootContentPage);

                // Add leaf content pages
                var leaContentPages = GetContentPageFaker(rootContentPage.Id).Generate(ContentPagesPerRootContentPages);
                foreach (var leaContentPage in leaContentPages)
                {
                    ContentPages.Add(leaContentPage);
                }
            }
        }

        private Faker<ContentPage> GetContentPageFaker(string parentContentPageId = null)
        {
            return new Faker<ContentPage>()
                .RuleFor(x => x.Id, f => f.UniqueIndex.ToString())
                .RuleFor(x => x.Handle, (f, x) => $"content-page-{x.Id}")
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath())
                .RuleFor(x => x.Title, f => f.Address.City())
                .RuleFor(x => x.Type, f => "page")
                .RuleFor(x => x.Description, (f, x) => $"{x.Title} - Description")
                .RuleFor(x => x.SeoTitle, (f, x) => $"{x.Title}")
                .RuleFor(x => x.SeoDescription, (f, x) => $"{x.Description}")
                .RuleFor(x => x.ParentContentPageId, f => parentContentPageId)
                .RuleFor(x => x.PrimaryImage, f => f.Image.PicsumUrl());
        }
    }
}
