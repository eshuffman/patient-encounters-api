using AutoMapper;
using HealthAPI.Data.Model;
using HealthAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAPI.DTOMappings
{
    public static class MapperExtensions
    {

        public static IEnumerable<EncounterDTO> MapEncountersToEncounterDtos(this IMapper mapper, IEnumerable<Encounter> encounters)
        {
            return encounters
                .Select(x => mapper.MapEncounterToEncounterDto(x))
                .ToList();
        }

        /// <summary>
        /// Helper method to build model for a encounter DTO.
        /// </summary>
        /// <param name="encounter">The encounter to be persisted.</param>
        /// <returns>A encounter DTO.</returns>
        public static EncounterDTO MapEncounterToEncounterDto(this IMapper mapper, Encounter encounter)
        {
            return new EncounterDTO()
            {
                Id = encounter.Id,
                PatientId = encounter.PatientId,
                Notes = encounter.Notes,
                VisitCode = encounter.VisitCode,
                Provider = encounter.Provider,
                BillingCode = encounter.BillingCode,
                Icd10 = encounter.Icd10,
                TotalCost = encounter.TotalCost,
                Copay = encounter.Copay,
                ChiefComplaint = encounter.ChiefComplaint,
                Pulse = encounter.Pulse,
                Systolic = encounter.Systolic,
                Diastolic = encounter.Diastolic,
                Date = encounter.Date
            };
        }

        public static Encounter MapEncounterDtoToEncounter(this IMapper mapper, EncounterDTO encounterDTO)
        {
            var encounter = new Encounter();

            encounter = mapper.Map(encounterDTO, encounter);

            return encounter;
        }
    }
}
