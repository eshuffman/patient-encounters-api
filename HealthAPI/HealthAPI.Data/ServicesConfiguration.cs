using HealthAPI.Data.Context;
using HealthAPI.Data.Interfaces;
using HealthAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HealthAPI.Data
{
    /// <summary>
    /// This class provides configuration options for services and uses
    /// appsettings.[current_environment].json file to configure context.
    /// </summary>
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<HealthCtx>(options =>
            {
                if (config.GetValue<bool>("IsDevelopment"))
                {
                    options.UseNpgsql("Host=localhost; Port=5432; Database=postgres_tests; UserName=postgres; Password=root");
                }
            });
            services.AddScoped<IHealthCtx>(provider => provider.GetService<HealthCtx>());

            services.AddScoped<IPatientRepository, PatientRepository>();

            services.AddScoped<IEncounterRepository, EncounterRepository>();

            return services;

        }

    }

}
