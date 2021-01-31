using System;
using System.Collections.Generic;
using Bogus;
using Google.Protobuf.WellKnownTypes;
using LightOps.Commerce.Proto.Types;

namespace Sample.ContentPageService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }

        public int RootEntities { get; set; } = 2;
        public int LeafEntities { get; set; } = 3;

        public IList<ContentPage> ContentPages { get; internal set; } = new List<ContentPage>();

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
                .RuleFor(x => x.Type, f => "page")
                .RuleFor(x => x.CreatedAt, f => Timestamp.FromDateTime(f.Date.Past(2).ToUniversalTime()))
                .RuleFor(x => x.UpdatedAt, f => Timestamp.FromDateTime(f.Date.Past().ToUniversalTime()))
                .RuleFor(x => x.PrimaryImage, f => new Image
                {
                    Id = $"gid://Image/1000{f.UniqueIndex}",
                    Url = f.Image.PicsumUrl(),
                    AltText = {GetLocalizedStrings(f.Lorem.Sentence())},
                    FocalCenterTop = f.Random.Double(0, 1),
                    FocalCenterLeft = f.Random.Double(0, 1),
                })
                .RuleFor(x => x.IsSearchable, f => true)
                .RuleFor(x => x.SortOrder, f => f.Random.Long(0, 10000))
                .FinishWith((f, x) =>
                {
                    var title = f.Address.City();
                    x.Titles.AddRange(GetLocalizedStrings(title));
                    x.Descriptions.AddRange(GetLocalizedStrings($"{title} - Description"));
                    x.Urls.AddRange(GetLocalizedStrings(f.Internet.UrlRootedPath(), true));
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
