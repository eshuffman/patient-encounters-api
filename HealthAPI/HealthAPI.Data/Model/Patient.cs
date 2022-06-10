namespace HealthAPI.Data.Model
{
    /// <summary>
    /// Describes a patient object.
    /// </summary>
    public class Patient : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Ssn { get; set; }

        public string Email { get; set; }

        public double? Age { get; set; }

        public double? Height { get; set; }

        public double? Weight { get; set; }

        public string Insurance { get; set; }

        public string Gender { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Postal { get; set; }

    }
}
