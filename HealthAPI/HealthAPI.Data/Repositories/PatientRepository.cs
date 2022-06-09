using HealthAPI.Data.Context;
using HealthAPI.Data.Interfaces;
using HealthAPI.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthAPI.Data.Filters;
using System.Linq;

namespace HealthAPI.Data.Repositories
{
    /// <summary>
    /// This class handles methods for making requests to the patient repository.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly IHealthCtx _ctx;

        public PatientRepository(IHealthCtx ctx)
        {
            _ctx = ctx;
        }


        public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            await _ctx.Patients.AddAsync(patient);
            await _ctx.SaveChangesAsync();

            return patient;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _ctx.Patients
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int? patientId)
        {
            return await _ctx.Patients
                .AsNoTracking()
                .WherePatientIdEquals(patientId)
                .SingleOrDefaultAsync();
        }

        public async Task<Patient> UpdatePatientAsync(Patient patient)
        {
            _ctx.Patients.Update(patient);
            await _ctx.SaveChangesAsync();

            return patient;
        }

        public async Task<Patient> DeletePatientByIdAsync(int patientId)
        {
            var patient = await _ctx.Patients
                .AsNoTracking()
                .Where(p => p.Id == patientId)
                .SingleOrDefaultAsync();
            _ctx.Patients.Remove(patient);
            await _ctx.SaveChangesAsync();
            return patient;
        }
    }
}
