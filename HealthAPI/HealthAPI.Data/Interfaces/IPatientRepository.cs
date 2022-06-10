using HealthAPI.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAPI.Data.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for patient repository methods.
    /// </summary>
    public interface IPatientRepository
    {
        Task<Patient> GetPatientByEmailAsync(Patient patient);
        Task<Patient> CreatePatientAsync(Patient patient);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int? patientId);
        Task<Patient> UpdatePatientAsync(Patient patient);
        Task<Patient> DeletePatientByIdAsync(int patientId);
    }
}
