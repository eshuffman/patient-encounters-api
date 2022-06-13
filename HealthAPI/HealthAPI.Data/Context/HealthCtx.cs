using HealthAPI.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HealthAPI.Data.Context
{
    /// <summary>
    /// Health database context provider.
    /// </summary>
    public class HealthCtx : DbContext, IHealthCtx
    {
        public HealthCtx(DbContextOptions<HealthCtx> options) : base(options)
        { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Encounter> Encounters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SeedData();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
