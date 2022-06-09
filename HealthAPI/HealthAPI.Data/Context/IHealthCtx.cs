using HealthAPI.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace HealthAPI.Data.Context
{
    /// <summary>
    /// This interface provides an abstraction layer for the Health database context.
    /// </summary>
    public interface IHealthCtx
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Encounter> Encounters { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
