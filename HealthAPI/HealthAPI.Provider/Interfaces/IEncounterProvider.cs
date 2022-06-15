using System.Collections.Generic;
using System.Threading.Tasks;
using HealthAPI.Data.Model;

namespace HealthAPI.Provider.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for patient encounter-related service methods.
    /// </summary>
    public interface IEncounterProvider
    {
        Task<Encounter> CreateEncounterAsync(int patientId, Encounter encounter);
        Task<IEnumerable<Encounter>> GetAllEncountersByIdAsync(int? patientId);
        Task<Encounter> GetEncounterByIdAsync(int encounterId);
        Task<Encounter> UpdateEncounterAsync(int encounterId, Encounter encounter, int patientId);

    }
}
