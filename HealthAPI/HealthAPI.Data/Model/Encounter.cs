using System.Collections.Generic;

namespace HealthAPI.Data.Model
{
    /// <summary>
    /// Describes a patient encounter object.
    /// </summary>
    public class Encounter : BaseEntity
    {
        public int? PatientId { get; set; }

        public string Notes { get; set; }

        public string VisitCode { get; set; }

        public string Provider { get; set; }

        public string BillingCode { get; set; }

        public string Icd10 { get; set; }

        public decimal? TotalCost { get; set; }

        public decimal? Copay { get; set; }

        public string ChiefComplaint { get; set; }

        public double? Pulse { get; set; }

        public double? Systolic { get; set; }

        public double? Diastolic { get; set; }

        public string Date { get; set; }

    }
}
