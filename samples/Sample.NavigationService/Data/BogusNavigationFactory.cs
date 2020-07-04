using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using LightOps.Commerce.Services.Navigation.Api.Models;
using LightOps.Commerce.Services.Navigation.Domain.Models;

namespace Sample.NavigationService.Data
{
    public class BogusNavigationFactory
    {
        public int? Seed { get; set; }

        public int RootNavigations { get; set; } = 2;
        public int NavigationsPerRootNavigation { get; set; } = 3;
        public int LinksPerLeafNavigation { get; set; } = 3;

        public IList<INavigation> Navigations { get; internal set; } = new List<INavigation>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }
            
            // Add root navigations
            var rootNavigations = GetNavigationFaker().Generate(RootNavigations);
            foreach (var rootNavigation in rootNavigations)
            {
                Navigations.Add(rootNavigation);

                // Add leaf navigations
                var leafNavigations = GetNavigationFaker(rootNavigation.Id).Generate(NavigationsPerRootNavigation);
                foreach (var leafNavigation in leafNavigations)
                {
                    Navigations.Add(leafNavigation);

                    // Add links
                    var links = GetNavigationLinkFaker().Generate(LinksPerLeafNavigation);
                    foreach (var link in links)
                    {
                        leafNavigation.Links.Add(link);
                    }
                }
            }
        }

        private Faker<Navigation> GetNavigationFaker(string parentNavigationId = null)
        {
            return new Faker<Navigation>()
                .RuleFor(x => x.Id, f => f.UniqueIndex.ToString())
                .RuleFor(x => x.Handle, (f, x) => $"navigation-{x.Id}")
                .RuleFor(x => x.Header, (f, x) => new NavigationLink
                {
                    Title = f.Commerce.Categories(1).First(),
                    Url = f.Internet.UrlRootedPath(),
                })
                .RuleFor(x => x.ParentNavigationId, f => parentNavigationId);
        }

        private Faker<NavigationLink> GetNavigationLinkFaker()
        {
            return new Faker<NavigationLink>()
                .RuleFor(x => x.Title, f => f.Commerce.ProductName())
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath());
        }
    }
}
