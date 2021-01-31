using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Configuration;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts;
using LightOps.DependencyInjection.Configuration;
using LightOps.Mapping.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sample.StorefrontGateway
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLightOpsDependencyInjection(root =>
            {
                root
                    .AddMapping()
                    .AddStorefrontGateway(services, gateway =>
                    {
                        // Configure languages
                        gateway.UseLanguages("en-US", "da-DK");

                        // Configure currencies
                        gateway.UseCurrencies("EUR", "DKK");

                        // Configure CDN
                        gateway.UseImageCdn("https://cdn.example.com");

                        // Configure service connections
                        gateway.UseContentPages("http://sample-content-page-service:80");
                        gateway.UseNavigations("http://sample-navigation-service:80");
                        gateway.UseMetaFields("http://sample-meta-field-service:80");
                        gateway.UseCategories("http://sample-category-service:80");
                        gateway.UseProducts("http://sample-product-service:80");

                        // Configure GraphQL
                        gateway.ConfigureGraphQL((options, provider) =>
                        {
                            options.EnableMetrics = true;
                        });
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use HTTP middleware at path /graphql
            app.UseGraphQL<ISchema>();

            // Enable GraphiQL at path /ui/graphiql
            app.UseGraphiQLServer(new GraphiQLOptions());
            // Enable Playground at path /ui/playground
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
        }
    }
}
