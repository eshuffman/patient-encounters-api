using System.Linq;
using HealthAPI.Data.Model;

namespace HealthAPI.Data.Filters
{
    /// <summary>
    /// Filter collection for patient encounters context queries.
    /// </summary>
    public static class MovieFilters
    {
        /// <summary>
        /// Returns the patient with a given Id.
        /// </summary>
        /// <param name="patients"></param>
        /// <param name="patientId"></param>
        /// <returns>The matching movie.</returns>
        public static IQueryable<Patient> WherePatientIdEquals(this IQueryable<Patient> patients, int? patientId)
        {
            return patients.Where(p => p.Id == patientId).AsQueryable();
        }

        public static IQueryable<Patient> WherePatientEmailEquals(this IQueryable<Patient> patients, string patientEmail)
        {
            return patients.Where(p => p.Email == patientEmail).AsQueryable();
        }
    }
}