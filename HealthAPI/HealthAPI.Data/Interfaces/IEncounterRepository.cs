using HealthAPI.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAPI.Data.Interfaces
{
    /// <summary>
    /// This interface provides an abstraction layer for patient encounter repository methods.
    /// </summary>
    public interface IEncounterRepository
    {

        Task<Encounter> CreateEncounterAsync(Encounter encounter);
        Task<List<Encounter>> GetAllEncountersByIdAsync(int? patientId);
        Task<Encounter> GetEncounterByIdAsync(int encounterId);
        Task<Encounter> UpdateEncounterAsync(Encounter encounter);
    }
}
