using LightOps.Commerce.Services.ContentPage.Backends.InMemory.Configuration;
using LightOps.Commerce.Services.ContentPage.Configuration;
using LightOps.Commerce.Services.ContentPage.Domain.Services.V1;
using LightOps.CQRS.Configuration;
using LightOps.DependencyInjection.Configuration;
using LightOps.Mapping.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.ContentPageService.Data;

namespace Sample.ContentPageService
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
                    .AddCqrs()
                    .AddContentPageService(service =>
                    {
                        service.UseInMemoryBackend(root, backend =>
                        {
                            var factory = new BogusContentFactory
                            {
                                Seed = 123,
                            };
                            factory.Generate();

                            backend
                                .UseContentPages(factory.ContentPages);
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
                endpoints.MapGrpcService<ContentPageGrpcService>();
            });
        }
    }
}