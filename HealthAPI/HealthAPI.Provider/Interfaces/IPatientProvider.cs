using System.Collections.Generic;
using System.Threading.Tasks;
using HealthAPI.Data.Model;

namespace HealthAPI.Provider.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for patient-related service methods.
    /// </summary>
    public interface IPatientProvider
    {
        Task<Patient> CreatePatientAsync(Patient model);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int patientId);
        Task<Patient> UpdatePatientAsync(int patientId, Patient patient);
        Task<Patient> DeletePatientByIdAsync(int patientId);

    }
}
