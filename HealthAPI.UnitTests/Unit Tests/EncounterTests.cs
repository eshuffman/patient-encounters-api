using HealthAPI.Data.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using HealthAPI.Provider.Providers;
using HealthAPI.Utilities.HttpResponseExceptions;
using FluentAssertions;

namespace HealthAPI.Testing.UnitTests
{
    public class EncounterProviderTests
    {
        private readonly Mock<IEncounterRepository> encounterRepositoryStub;
        private readonly Mock<IPatientRepository> patientRepositoryStub;
        private readonly Mock<ILogger<EncounterProvider>> loggerStub;
        private readonly Mock<ILogger<PatientProvider>> patientLoggerStub;
        private readonly EncounterProvider encounterProvider;
        private readonly PatientProvider patientProvider;

        private readonly Data.Model.Patient testPatient1;
        private readonly Data.Model.Patient testPatient2;
        private readonly Data.Model.Patient testPatient3;
        private readonly Data.Model.Encounter testEncounter1;
        private readonly Data.Model.Encounter testEncounter2;
        private readonly Data.Model.Encounter testEncounter3;
        private readonly Data.Model.Encounter testEncounterAltered;

        private readonly List<Data.Model.Patient> testPatientDatabase;
        private readonly List<Data.Model.Encounter> testEncounterDatabase;


        public EncounterProviderTests()
        {
            // Arrange
            // Create some patients 
            encounterRepositoryStub = new Mock<IEncounterRepository>();
            patientRepositoryStub = new Mock<IPatientRepository>();
            loggerStub = new Mock<ILogger<EncounterProvider>>();
            patientLoggerStub = new Mock<ILogger<PatientProvider>>();
            encounterProvider = new EncounterProvider(encounterRepositoryStub.Object, patientRepositoryStub.Object, loggerStub.Object);
            patientProvider = new PatientProvider(patientRepositoryStub.Object, encounterRepositoryStub.Object, patientLoggerStub.Object);

            testPatient1 = new Data.Model.Patient
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
            };

            testPatient2 = new Data.Model.Patient
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
            };

            testPatient3 = new Data.Model.Patient
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
            };

            testEncounter1 = new Data.Model.Encounter
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
            };

            testEncounter2 = new Data.Model.Encounter
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
            };

            testEncounter3 = new Data.Model.Encounter
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
            };

            testEncounterAltered = new Data.Model.Encounter
            {
                Id = 1,
                PatientId = 1,
                Notes = "Says he's happy to be alive",
                VisitCode = "A1A 2B2",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "111.222.333-44",
                Icd10 = "A11",
                TotalCost = 99.99m,
                Copay = 20.01m,
                ChiefComplaint = "Overwhelming sense of glee",
                Pulse = 122,
                Systolic = 123,
                Diastolic = 90,
                Date = "1796-02-21",
            };

            testPatientDatabase = new List<Data.Model.Patient> { testPatient1, testPatient2, testPatient3 };
            testEncounterDatabase = new List<Data.Model.Encounter> { testEncounter1, testEncounter2, testEncounter3 };

            encounterRepositoryStub.Setup(x => x.CreateEncounterAsync(testEncounter1)).ReturnsAsync(testEncounter1);
            encounterRepositoryStub.Setup(x => x.GetAllEncountersByIdAsync(1)).ReturnsAsync(testEncounterDatabase);
            patientRepositoryStub.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(testPatientDatabase);
            patientRepositoryStub.Setup(x => x.GetPatientByIdAsync(1)).ReturnsAsync(testPatient1);
            encounterRepositoryStub.Setup(x => x.GetEncounterByIdAsync(1)).ReturnsAsync(testEncounter1);
            encounterRepositoryStub.Setup(x => x.UpdateEncounterAsync(testEncounterAltered)).ReturnsAsync(testEncounterAltered);

        }

        [Fact]
        public async Task CreateEncounterAsync_WithNoDatabase_ThrowsException()
        {
            encounterRepositoryStub.Setup(x => x.CreateEncounterAsync(testEncounter1)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await encounterProvider.CreateEncounterAsync(1, testEncounter1); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task CreateEncounterAsync_WithValidInfo_ReturnsDto()
        {
            var result = await encounterProvider.CreateEncounterAsync(1, testEncounter1);
            result.Should().BeEquivalentTo(testEncounter1,
                options => options.ComparingByMembers<Data.Model.Encounter>());

        }

        [Fact]
        public async Task CreateEncounterAsync_WithInvalidInfo_ThrowsError()
        {
            var badTestEncounter = new Data.Model.Encounter
            {
                PatientId = 2,
                Notes = "Patient keeps clutching head",
                VisitCode = "A",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "1",
                Icd10 = "A",
                TotalCost = 1,
                Copay = 1,
                ChiefComplaint = "Migraine for days",
                Pulse = -1,
                Systolic = 1.1,
                Diastolic = 2.2,
                Date = "1",
            };

            Func<Task> result = async () => { await encounterProvider.CreateEncounterAsync(1, badTestEncounter); };
            //Assert
            await result.Should().ThrowAsync<BadRequestException>();

        }

        [Fact]
        public async Task GetAllEncountersByIdAsync_WithNoDatabase_ThrowsException()
        {
            encounterRepositoryStub.Setup(x => x.GetAllEncountersByIdAsync(1)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await encounterProvider.GetAllEncountersByIdAsync(1); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task GetAllEncountersByIdAsync_WithEncountersInDatabase_ReturnsEncounters()
        {
            var result = await encounterProvider.GetAllEncountersByIdAsync(1);
            result.Should().BeEquivalentTo(testEncounterDatabase,
                options => options.ComparingByMembers<Data.Model.Encounter>());
        }

        [Fact]
        public async Task GetAllEncountersByIdAsync_WithNoEncountersInDatabase_ReturnsEmptyList()
        {

            var emptyTestEncounterDatabase = new List<Data.Model.Encounter> { };

            encounterRepositoryStub.Setup(x => x.GetAllEncountersByIdAsync(1)).ReturnsAsync(emptyTestEncounterDatabase);

            var result = await encounterProvider.GetAllEncountersByIdAsync(1);
            result.Should().BeEquivalentTo(emptyTestEncounterDatabase);
        }

        [Fact]
        public async Task GetEncounterByIdAsync_WithValidId_ReturnsEncounter()
        {
            var result = await encounterProvider.GetEncounterByIdAsync(1);
            result.Should().BeEquivalentTo(testEncounter1,
                options => options.ComparingByMembers<Data.Model.Encounter>());
        }

        [Fact]
        public async Task GetEncounterByIdAsync_WithNonexistentId_ThrowsError()
        {
            Func<Task> result = async () => { await encounterProvider.GetEncounterByIdAsync(100); };
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateEncounterAsync_WithNoDatabase_ThrowsException()
        {
            encounterRepositoryStub.Setup(x => x.UpdateEncounterAsync(testEncounterAltered)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await encounterProvider.UpdateEncounterAsync(1, testEncounterAltered); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }
        [Fact]
        public async Task UpdateEncounterAsync_WithValidInfo_ReturnsUpdatedDto()
        {

            var result = await encounterProvider.UpdateEncounterAsync(1, testEncounterAltered);
            result.Should().BeEquivalentTo(testEncounterAltered,
                options => options.ComparingByMembers<Data.Model.Encounter>());

        }

        [Fact]
        public async Task UpdateEncounterAsync_WithNonexistentId_ThrowsError()
        {
            Func<Task> result = async () => { await encounterProvider.UpdateEncounterAsync(100, testEncounterAltered); };
            await result.Should().ThrowAsync<NotFoundException>();

        }

        [Fact]
        public async Task UpdatePatientAsync_WithInvalidInfo_ThrowsError()
        {
            var badTestEncounterAltered = new Data.Model.Encounter
            {
                Id = 1,
                PatientId = 1,
                Notes = "Says he's happy to be alive",
                VisitCode = "A",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "1",
                Icd10 = "A1",
                TotalCost = 22,
                Copay = -1,
                ChiefComplaint = "Overwhelming sense of glee",
                Pulse = 1.1,
                Systolic = -1,
                Diastolic = -1,
                Date = "1",
            };
            Func<Task> result = async () => { await encounterProvider.UpdateEncounterAsync(1, badTestEncounterAltered); };
            await result.Should().ThrowAsync<BadRequestException>();

        }

        [Fact]
        public async Task GetPatientByIdAsync_WithNoPatientDatabase_ThrowsException()
        {
            patientRepositoryStub.Setup(x => x.GetPatientByIdAsync(1)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await patientProvider.GetPatientByIdAsync(1); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task GetPatientByIdAsync_WithNonexistentId_ThrowsError()
        {
            Func<Task> result = async () => { await patientProvider.GetPatientByIdAsync(100); };
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public void ValidateIfEmptyOrNull_ReturnsTrueIfEmpty()
        {
            var given = string.Empty;
            var actual = encounterProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfEmptyOrNull_ReturnsTrueIfNull()
        {
            string given = null;
            var actual = encounterProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfEmptyOrNull_ReturnsTrueIfOnlySpaces()
        {
            var given = "   ";
            var actual = encounterProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfEmptyOrNull_ReturnsFalseIfNotEmptyOrNull()
        {
            var given = "Close encounters of the patient kind...";
            var actual = encounterProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeFalse();
        }
    }
}