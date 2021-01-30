using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Google.Protobuf.WellKnownTypes;
using LightOps.Commerce.Proto.Types;

namespace Sample.NavigationService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }

        public int RootEntities { get; set; } = 2;
        public int LeafEntities { get; set; } = 3;
        public int LinksPerLeafEntity { get; set; } = 3;

        public IList<Navigation> Navigations { get; internal set; } = new List<Navigation>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            // Add root entities
            var rootEntities = GetNavigationFaker().Generate(RootEntities);
            foreach (var rootEntity in rootEntities)
            {
                Navigations.Add(rootEntity);

                // Add leaf entities
                var leafEntities = GetSubNavigationFaker().Generate(LeafEntities);
                foreach (var leafEntity in leafEntities)
                {
                    rootEntity.SubNavigations.Add(leafEntity);

                    // Add links
                    var links = GetNavigationLinkFaker().Generate(LinksPerLeafEntity);
                    foreach (var link in links)
                    {
                        leafEntity.Links.Add(link);
                    }
                }
            }
        }

        private Faker<Navigation> GetNavigationFaker()
        {
            return new Faker<Navigation>()
                .RuleFor(x => x.Id, f => $"gid://Navigation/{f.UniqueIndex}")
                .RuleFor(x => x.ParentId, f => "gid://")
                .RuleFor(x => x.Handle, (f, x) => $"navigation-{f.UniqueIndex}")
                .RuleFor(x => x.Type, f => "navigation")
                .RuleFor(x => x.CreatedAt, f => Timestamp.FromDateTime(f.Date.Past(2).ToUniversalTime()))
                .RuleFor(x => x.UpdatedAt, f => Timestamp.FromDateTime(f.Date.Past().ToUniversalTime()))
                .RuleFor(x => x.Header, (f, x) => new NavigationLink
                {
                    Titles = {GetLocalizedStrings(f.Commerce.Categories(1).First())},
                    Urls = {GetLocalizedStrings(f.Internet.UrlRootedPath(), true)},
                });
        }

        private Faker<SubNavigation> GetSubNavigationFaker()
        {
            return new Faker<SubNavigation>()
                .RuleFor(x => x.Header, (f, x) => new NavigationLink
                {
                    Titles = {GetLocalizedStrings(f.Commerce.Categories(1).First())},
                    Urls = {GetLocalizedStrings(f.Internet.UrlRootedPath(), true)},
                });
        }

        private Faker<NavigationLink> GetNavigationLinkFaker()
        {
            return new Faker<NavigationLink>()
                .FinishWith((f, x) =>
                {
                    x.Titles.AddRange(GetLocalizedStrings(f.Commerce.ProductName()));
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
