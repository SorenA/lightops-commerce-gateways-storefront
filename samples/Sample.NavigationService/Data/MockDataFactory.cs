using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using LightOps.Commerce.Services.Navigation.Api.Models;
using LightOps.Commerce.Services.Navigation.Domain.Models;

namespace Sample.NavigationService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }

        public int RootEntities { get; set; } = 2;
        public int LeafEntities { get; set; } = 3;
        public int LinksPerLeafEntity { get; set; } = 3;

        public IList<INavigation> Navigations { get; internal set; } = new List<INavigation>();

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
                .RuleFor(x => x.CreatedAt, f => f.Date.Past(2))
                .RuleFor(x => x.UpdatedAt, f => f.Date.Past())
                .RuleFor(x => x.Header, (f, x) => new NavigationLink
                {
                    Title = f.Commerce.Categories(1).First(),
                    Url = f.Internet.UrlRootedPath(),
                });
        }

        private Faker<SubNavigation> GetSubNavigationFaker()
        {
            return new Faker<SubNavigation>()
                .RuleFor(x => x.Header, (f, x) => new NavigationLink
                {
                    Title = f.Commerce.Categories(1).First(),
                    Url = f.Internet.UrlRootedPath(),
                });
        }

        private Faker<NavigationLink> GetNavigationLinkFaker()
        {
            return new Faker<NavigationLink>()
                .RuleFor(x => x.Title, f => f.Commerce.ProductName())
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath());
        }
    }
}
