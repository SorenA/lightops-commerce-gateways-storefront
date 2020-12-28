using System;
using System.Collections.Generic;
using Bogus;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Domain.Models;
using NodaMoney;

namespace Sample.ProductService.Data
{
    public class MockDataFactory
    {
        public int? Seed { get; set; }

        public int ProductsPerCategory { get; set; } = 5;
        public int VariantsPerProduct { get; set; } = 3;
        public int ImagesPerProduct { get; set; } = 1;
        public int ImagesPerProductVariant { get; set; } = 1;

        public IList<IProduct> Products { get; internal set; } = new List<IProduct>();

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
                .RuleFor(x => x.Title, f => f.Commerce.ProductName())
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath())
                .RuleFor(x => x.Type, f => "product")
                .RuleFor(x => x.Description, (f, x) => $"{x.Title} - Description")
                .RuleFor(x => x.CreatedAt, f => f.Date.Past(2))
                .RuleFor(x => x.UpdatedAt, f => f.Date.Past())
                .RuleFor(x => x.PrimaryCategoryId, f => primaryCategoryId)
                .FinishWith((f, x) =>
                {
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
                .RuleFor(x => x.Title, f => f.Commerce.Color())
                .RuleFor(x => x.Sku, f => f.Commerce.Ean8())
                .RuleFor(x => x.UnitPrice, f => new Money(f.Finance.Amount(10), "EUR"));
        }

        private Faker<Image> GetImageFaker()
        {
            return new Faker<Image>()
                .RuleFor(x => x.Id, f => $"gid://Image/{f.UniqueIndex}")
                .RuleFor(x => x.Url, f => f.Image.PicsumUrl())
                .RuleFor(x => x.AltText, f => f.Commerce.ProductAdjective())
                .RuleFor(x => x.FocalCenterTop, f => f.Random.Double(0, 1))
                .RuleFor(x => x.FocalCenterLeft, f => f.Random.Double(0, 1));
        }
    }
}
