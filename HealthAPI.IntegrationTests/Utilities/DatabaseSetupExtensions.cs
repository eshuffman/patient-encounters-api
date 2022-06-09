
using MovieRentalsAPI.Data.Context;

namespace MovieRentalsAPI.IntegrationTesting.Utilities
{
    public static class DatabaseSetupExtensions
    {
        public static void InitializeDatabaseForTests(this MovieCtx context)
        {
            context.Movies.AddRange(context.Movies);
            context.Rentals.AddRange(context.Rentals);
            context.SaveChanges();
        }

        public static void ReinitializeDatabaseForTests(this MovieCtx context)
        {
            context.Movies.RemoveRange(context.Movies);
            context.Rentals.RemoveRange(context.Rentals);
            context.InitializeDatabaseForTests();
        }
    }
}
