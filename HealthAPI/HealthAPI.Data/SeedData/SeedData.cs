using HealthAPI.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HealthAPI.Data.Context
{
    public static class Extensions
    {
        /// <summary>
        /// Produces a set of seed data to insert into the database on startup.
        /// </summary>
        /// <param name="modelBuilder">Used to build model base DbContext.</param>
        public static void SeedData(this ModelBuilder modelBuilder)
        {

            var patients = new List<Patient>()
            {
                new Patient()
            {
                Id = 1,
                FirstName = "Fitzwilliam",
                LastName = "Darcy",
                Ssn = "123-12-1234",
                Email = "prideful@longbourne.house",
                Street = "123 Longbourne Circle",
                City = "Longbourne",
                State = "TN",
                Postal = "12321",
                Age = 35,
                Height = 72,
                Weight = 200,
                Insurance = "Queen's Care",
                Gender = "Male",
            },
            new Patient()
                {
                Id = 2,
                FirstName = "Elizabeth",
                LastName = "Bennett",
                Ssn = "123-12-1234",
                Email = "sunny@longbourne.house",
                Street = "123 Longbourne Circle",
                City = "Longbourne",
                State = "TN",
                Postal = "12321",
                Age = 21,
                Height = 64,
                Weight = 120,
                Insurance = "Queen's Care",
                Gender = "Female",
            },
                new Patient()
            {
                Id = 3,
                    FirstName = "Mary",
                    LastName = "Bennett",
                    Ssn = "123-12-1234",
                    Email = "misunderstood@longbourne.house",
                    Street = "123 Longbourne Circle",
                    City = "Longbourne",
                    State = "TN",
                    Postal = "12321",
                    Age = 18,
                    Height = 63,
                    Weight = 128,
                    Insurance = "Queen's Care",
                    Gender = "Other",
                }
            };

            modelBuilder.Entity<Patient>().HasData(patients);

            var encounters = new List<Encounter>()
            {
                new Encounter()
            {
                Id = 1,
                PatientId = 1,
                Notes = "Distant look in eyes",
                VisitCode = "A1A 2B2",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "111.222.333-44",
                Icd10 = "A11",
                TotalCost = 99.99m,
                Copay = 20.01m,
                ChiefComplaint = "Heartache",
                Pulse = 122,
                Systolic = 123,
                Diastolic = 90,
                Date = "1796-02-21",
            },

            new Encounter()
            {
                Id = 2,
                PatientId = 1,
                Notes = "Unusually cheery",
                VisitCode = "A1A 2B2",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "111.222.333-44",
                Icd10 = "A11",
                TotalCost = 99.99m,
                Copay = 20.01m,
                ChiefComplaint = "Faintness",
                Pulse = 122,
                Systolic = 123,
                Diastolic = 90,
                Date = "1796-05-21",
            },

            new Encounter()
            {
                Id = 3,
                PatientId = 1,
                Notes = "Patient keeps clutching head",
                VisitCode = "A1A 2B2",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "111.222.333-44",
                Icd10 = "A11",
                TotalCost = 99.99m,
                Copay = 20.01m,
                ChiefComplaint = "Migraine for days",
                Pulse = 122,
                Systolic = 123,
                Diastolic = 90,
                Date = "1796-02-21",
            }
            };

            modelBuilder.Entity<Encounter>().HasData(encounters);
        }
    }
}

