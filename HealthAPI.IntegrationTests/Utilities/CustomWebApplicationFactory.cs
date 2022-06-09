using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MovieRentalsAPI.API;
using MovieRentalsAPI.Data.Context;
using System;

namespace MovieRentalsAPI.IntegrationTesting.Utilities
{
    /// <summary>
    /// A custom WebApplicationFactory class to initilize test database
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// Overides WebApplicationFactory method with custom configuration services
        /// </summary>
        /// <param name="builder">IWebHostBuilder</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(MovieCtx));

                services.AddDbContext<MovieCtx>((options, context) =>
                {
                    context.UseNpgsql("Host=localhost; Port=5432; Database=postgres_tests; UserName=postgres; Password=root");
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<MovieCtx>();
                var logger = scopedServices.GetRequiredService<ILogger<WebApplicationFactory<Startup>>>();

                context.Database.EnsureCreated();

                try
                {
                    context.ReinitializeDatabaseForTests();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured seeding the database with the message: " + ex.Message);
                }
            });
        }
    }
}
