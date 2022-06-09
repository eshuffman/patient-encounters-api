using HealthAPI.Provider.Interfaces;
using HealthAPI.Provider.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace HealthAPI.Provider
{
    /// <summary>
    /// This class provides configuration options for provider services.
    /// </summary>
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services.AddScoped<IPatientProvider, PatientProvider>();
            services.AddScoped<IEncounterProvider, EncounterProvider>();

            return services;
        }

    }
}
