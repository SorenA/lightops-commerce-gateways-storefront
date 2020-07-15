using System;
using System.Collections.Generic;
using Bogus;
using LightOps.Commerce.Services.Product.Api.Models;
using LightOps.Commerce.Services.Product.Domain.Models;
using NodaMoney;

namespace Sample.ProductService.Data
{
    public class BogusProductFactory
    {
        public int? Seed { get; set; }

        public int ProductsPerCategory { get; set; } = 5;
        public int VariantsPerProduct { get; set; } = 3;

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
                var products = GetProductFaker(categoryId.ToString(), new[] { "0", "1", "2" }).Generate(ProductsPerCategory);

                foreach (var product in products)
                {
                    // Add variants
                    var variants = GetProductVariantFaker(product.Id).Generate(VariantsPerProduct);
                    foreach (var productVariant in variants)
                    {

                        product.Variants.Add(productVariant);
                    }

                    Products.Add(product);
                }
            }
        }

        private Faker<Product> GetProductFaker(string primaryCategoryId, string[] extraCategoryIds)
        {
            return new Faker<Product>()
                .RuleFor(x => x.Id, f => f.UniqueIndex.ToString())
                .RuleFor(x => x.Handle, (f, x) => $"product-{x.Id}")
                .RuleFor(x => x.Url, f => f.Internet.UrlRootedPath())
                .RuleFor(x => x.Title, f => f.Commerce.ProductName())
                .RuleFor(x => x.Type, f => "product")
                .RuleFor(x => x.Description, (f, x) => $"{x.Title} - Description")
                .RuleFor(x => x.SeoTitle, (f, x) => $"{x.Title}")
                .RuleFor(x => x.SeoDescription, (f, x) => $"{x.Description}")
                .RuleFor(x => x.PrimaryCategoryId, f => primaryCategoryId)
                .RuleFor(x => x.PrimaryImage, f => f.Image.PicsumUrl())
                .FinishWith((f, x) =>
                {
                    // Add images
                    x.Images.Add(x.PrimaryImage);
                    x.Images.Add(f.Image.PicsumUrl());
                    x.Images.Add(f.Image.PicsumUrl());

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
                .RuleFor(x => x.Id, f => f.UniqueIndex.ToString())
                .RuleFor(x => x.ProductId, f => productId)
                .RuleFor(x => x.Title, f => f.Commerce.Color())
                .RuleFor(x => x.Sku, f => f.Commerce.Ean8())
                .RuleFor(x => x.Price, f => new Money(f.Finance.Amount(10), "EUR"));
        }
    }
}
