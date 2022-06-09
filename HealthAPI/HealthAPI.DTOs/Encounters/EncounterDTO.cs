using System.Collections.Generic;

namespace HealthAPI.DTOs
{
    /// <summary>
    /// Describes a data transfer object for a patient encounter.
    /// </summary>
    public class EncounterDTO
    {
        public int? Id { get; set; }

        public int? PatientId { get; set; }

        public string Notes { get; set; }

        public string VisitCode { get; set; }

        public string Provider { get; set; }

        public string BillingCode { get; set; }

        public string Icd10 { get; set; }

        public decimal TotalCost { get; set; }

        public decimal Copay { get; set; }

        public string ChiefComplaint { get; set; }

        public int? Pulse { get; set; }

        public int? Systolic { get; set; }

        public int? Diastolic { get; set; }

        public string Date { get; set; }
    }
}
