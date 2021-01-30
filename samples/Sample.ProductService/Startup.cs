using LightOps.Commerce.Services.Product.Backends.InMemory.Configuration;
using LightOps.Commerce.Services.Product.Configuration;
using LightOps.Commerce.Services.Product.Domain.GrpcServices;
using LightOps.CQRS.Configuration;
using LightOps.DependencyInjection.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.ProductService.Data;

namespace Sample.ProductService
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
                    .AddCqrs()
                    .AddProductService(service =>
                    {
                        service.UseInMemoryBackend(root, backend =>
                        {
                            var factory = new MockDataFactory
                            {
                                Seed = 123,
                            };
                            factory.Generate();

                            backend
                                .UseProducts(factory.Products);
                        });
                    });
            });

            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<HealthGrpcService>();
                endpoints.MapGrpcService<ProductGrpcService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Sample ProductService. Communication must be made through a gRPC client.");
                });
            });
        }
    }
}
