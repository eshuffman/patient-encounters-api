using HealthAPI.Data.Context;
using HealthAPI.Data.Interfaces;
using HealthAPI.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HealthAPI.Data.Repositories
{
    /// <summary>
    /// This class handles methods for making requests to the patient encounters repository.
    /// </summary>
    public class EncounterRepository : IEncounterRepository
    {
        private readonly IHealthCtx _ctx;

        public EncounterRepository(IHealthCtx ctx)
        {
            _ctx = ctx;
        }


        public async Task<Encounter> CreateEncounterAsync(Encounter encounter)
        {
            await _ctx.Encounters.AddAsync(encounter);
            await _ctx.SaveChangesAsync();
            return encounter;
        }

        public async Task<List<Encounter>> GetAllEncountersByIdAsync(int? patientId)
        {
            return await _ctx.Encounters
                .AsNoTracking()
                .Where(e => e.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<Encounter> GetEncounterByIdAsync(int encounterId)
        {
            return await _ctx.Encounters
                .AsNoTracking()
                .Where(e => e.Id == encounterId)
                .SingleOrDefaultAsync();
        }

        public async Task<Encounter> UpdateEncounterAsync(Encounter encounter)
        {
            _ctx.Encounters.Update(encounter);
            await _ctx.SaveChangesAsync();

            return encounter;
        }
    }
}
