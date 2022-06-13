using HealthAPI.Data.Context;

namespace HealthAPI.IntegrationTests.Utilities
{
    public static class DatabaseSetupExtensions
    {
        public static void InitializeDatabaseForTests(this HealthCtx context)
        {
            context.Patients.AddRange(context.Patients);
            context.Encounters.AddRange(context.Encounters);
            context.SaveChanges();
        }

        public static void ReinitializeDatabaseForTests(this HealthCtx context)
        {
            context.Patients.RemoveRange(context.Patients);
            context.Encounters.RemoveRange(context.Encounters);
            context.InitializeDatabaseForTests();
        }
    }
}
