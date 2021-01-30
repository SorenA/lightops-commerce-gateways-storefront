using System;
using System.Collections.Generic;
using Bogus;
using Google.Protobuf.WellKnownTypes;
using LightOps.Commerce.Proto.Types;

namespace Sample.ProductService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }

        public int ProductsPerCategory { get; set; } = 5;
        public int VariantsPerProduct { get; set; } = 3;
        public int ImagesPerProduct { get; set; } = 1;
        public int ImagesPerProductVariant { get; set; } = 1;

        public IList<Product> Products { get; internal set; } = new List<Product>();

        public void Generate()
        {
            if (Seed.HasValue)
            {
                Randomizer.Seed = new Random(Seed.Value);
            }

            for (var categoryId = 0; categoryId < 30; categoryId++)
            {
                // Add products
                var products = GetProductFaker($"gid://Category/{categoryId}", new[]
                    {
                        "gid://Category/0",
                        "gid://Category/1",
                        "gid://Category/2",
                    })
                    .Generate(ProductsPerCategory);

                foreach (var product in products)
                {
                    // Add images
                    var images = GetImageFaker().Generate(ImagesPerProduct);
                    foreach (var image in images)
                    {
                        product.Images.Add(image);
                    }

                    // Add variants
                    var variants = GetProductVariantFaker(product.Id).Generate(VariantsPerProduct);
                    foreach (var productVariant in variants)
                    {

                        product.Variants.Add(productVariant);

                        // Add images
                        var variantImages  = GetImageFaker().Generate(ImagesPerProductVariant);
                        foreach (var variantImage in variantImages)
                        {
                            productVariant.Images.Add(variantImage);
                        }
                    }

                    Products.Add(product);
                }
            }
        }

        private Faker<Product> GetProductFaker(string primaryCategoryId, string[] extraCategoryIds)
        {
            return new Faker<Product>()
                .RuleFor(x => x.Id, f => $"gid://Product/{f.UniqueIndex}")
                .RuleFor(x => x.ParentId, f => "gid://")
                .RuleFor(x => x.Handle, (f, x) => $"product-{f.UniqueIndex}")
                .RuleFor(x => x.Type, f => "product")
                .RuleFor(x => x.CreatedAt, f => Timestamp.FromDateTime(f.Date.Past(2).ToUniversalTime()))
                .RuleFor(x => x.UpdatedAt, f => Timestamp.FromDateTime(f.Date.Past().ToUniversalTime()))
                .RuleFor(x => x.PrimaryCategoryId, f => primaryCategoryId)
                .RuleFor(x => x.IsSearchable, f => true)
                .FinishWith((f, x) =>
                {
                    var title = f.Commerce.ProductName();
                    x.Titles.AddRange(GetLocalizedStrings(title));
                    x.Descriptions.AddRange(GetLocalizedStrings($"{title} - Description"));
                    x.Urls.AddRange(GetLocalizedStrings(f.Internet.UrlRootedPath(), true));

                    // Add categories
                    x.CategoryIds.Add(primaryCategoryId);
                    foreach (var extraCategoryId in extraCategoryIds)
                    {
                        if (!x.CategoryIds.Contains(extraCategoryId))
                        {
                            x.CategoryIds.Add(extraCategoryId);
                        }
                    }
                });
        }

        private Faker<ProductVariant> GetProductVariantFaker(string productId)
        {
            return new Faker<ProductVariant>()
                .RuleFor(x => x.Id, f => $"gid://ProductVariant/{f.UniqueIndex}")
                .RuleFor(x => x.ProductId, f => productId)
                .RuleFor(x => x.Sku, f => f.Commerce.Ean8())
                .FinishWith((f, x) =>
                {
                    x.Titles.AddRange(GetLocalizedStrings(f.Commerce.Color()));
                    x.UnitPrices.AddRange(GetPrices(f.Finance.Amount(10)));
                });
        }

        private Faker<Image> GetImageFaker()
        {
            return new Faker<Image>()
                .RuleFor(x => x.Id, f => $"gid://Image/{f.UniqueIndex}")
                .RuleFor(x => x.Url, f => f.Image.PicsumUrl())
                .RuleFor(x => x.FocalCenterTop, f => f.Random.Double(0, 1))
                .RuleFor(x => x.FocalCenterLeft, f => f.Random.Double(0, 1))
                .FinishWith((f, x) =>
                {
                    x.AltText.AddRange(GetLocalizedStrings(f.Commerce.ProductAdjective()));
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

        private IList<Money> GetPrices(decimal amount)
        {
            var units = decimal.ToInt64(amount);
            var nanoUnits = decimal.ToInt32((amount - units) * 1_000_000_000);

            return new List<Money>
            {
                new Money
                {
                    CurrencyCode = "EUR",
                    Units = units,
                    Nanos = nanoUnits,
                },
                new Money
                {
                    CurrencyCode = "DKK",
                    Units = units,
                    Nanos = nanoUnits,
                }
            };
        }
    }
}