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
    public class PatientProviderTests
    {
        private readonly Mock<IPatientRepository> patientRepositoryStub;
        private readonly Mock<ILogger<PatientProvider>> loggerStub;
        private readonly PatientProvider patientProvider;
        private readonly Mock<IEncounterRepository> encounterRepositoryStub;

        private readonly Data.Model.Patient testPatient1;
        private readonly Data.Model.Patient testPatient2;
        private readonly Data.Model.Patient testPatient3;
        private readonly Data.Model.Patient testPatientAltered;
        private readonly Data.Model.Encounter testEncounter;
        private readonly List<Data.Model.Patient> testPatientDatabase;
        private readonly List<Data.Model.Encounter> testEncounterDatabase;


        public PatientProviderTests()
        {
            // Arrange
            // Create some movies 
            patientRepositoryStub = new Mock<IPatientRepository>();
            loggerStub = new Mock<ILogger<PatientProvider>>();
            encounterRepositoryStub = new Mock<IEncounterRepository>();

            patientProvider = new PatientProvider(patientRepositoryStub.Object, encounterRepositoryStub.Object, loggerStub.Object);

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

            testPatientAltered = new Data.Model.Patient
            {
                Id = 1,
                FirstName = "Fitzwilhelm",
                LastName = "Darcy",
                Ssn = "123-12-1234",
                Email = "prideful@longbourne.house",
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

            testEncounter = new Data.Model.Encounter
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

            testEncounterDatabase = new List<Data.Model.Encounter> { testEncounter };
            testPatientDatabase = new List<Data.Model.Patient> { testPatient1, testPatient2, testPatient3 };
            patientRepositoryStub.Setup(x => x.CreatePatientAsync(testPatient1)).ReturnsAsync(testPatient1);
            patientRepositoryStub.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(testPatientDatabase);
            patientRepositoryStub.Setup(x => x.GetPatientByIdAsync(1)).ReturnsAsync(testPatient1);
            patientRepositoryStub.Setup(x => x.UpdatePatientAsync(testPatientAltered)).ReturnsAsync(testPatientAltered);
            patientRepositoryStub.Setup(x => x.DeletePatientByIdAsync(3)).ReturnsAsync(testPatient3);
            patientRepositoryStub.Setup(x => x.GetPatientByEmailAsync(testPatient1)).ReturnsAsync(testPatient1);
            encounterRepositoryStub.Setup(x => x.GetAllEncountersByIdAsync(1)).ReturnsAsync(testEncounterDatabase);

        }

        [Fact]
        public async Task CreatePatientAsync_WithNoPatientDatabase_ThrowsException()
        {
            var testPatient4 = new Data.Model.Patient
            {
                Id = 22,
                FirstName = "Elizabeth",
                LastName = "Bennett",
                Ssn = "123-12-1234",
                Email = "chillaf@longbourne.house",
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
            patientRepositoryStub.Setup(x => x.CreatePatientAsync(testPatient4)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await patientProvider.CreatePatientAsync(testPatient4); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task CreatePatientAsync_WithNoEmailCheckDatabase_ThrowsException()
        {
            patientRepositoryStub.Setup(x => x.GetPatientByEmailAsync(testPatient1)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await patientProvider.CreatePatientAsync(testPatient1); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task CreatePatientAsync_WithValidInfo_ReturnsDto()
        {            
            var testPatient4 = new Data.Model.Patient
            {
                Id = 22,
                FirstName = "Elizabeth",
                LastName = "Bennett",
                Ssn = "123-12-1234",
                Email = "chillaf@longbourne.house",
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
            patientRepositoryStub.Setup(x => x.CreatePatientAsync(testPatient4)).ReturnsAsync(testPatient4);
            var result = await patientProvider.CreatePatientAsync(testPatient4);
            result.Should().BeEquivalentTo(testPatient4,
                options => options.ComparingByMembers<Data.Model.Patient>());

        }

        [Fact]
        public async Task CreatePatientAsync_WithInvalidInfo_ThrowsException()
        {
            //Arrange
            var badTestPatient = new Data.Model.Patient
            {
                FirstName = "1",
                LastName = "1",
                Ssn = "1",
                Email = "pridefullongbourne.house",
                Street = "",
                City = "",
                State = "TNT",
                Postal = "1",
                Age = 35.5,
                Height = 72.1,
                Weight = -1,
                Insurance = "",
                Gender = "Android",
            };
            patientRepositoryStub.Setup(x => x.CreatePatientAsync(badTestPatient)).ReturnsAsync(badTestPatient);
            //Act
            Func<Task> result = async () => { await patientProvider.CreatePatientAsync(badTestPatient); };
            //Assert
            await result.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task CreatePatientAsync_WithMissingInfo_ThrowsException()
        {
            //Arrange
            var badTestPatient = new Data.Model.Patient {};
            patientRepositoryStub.Setup(x => x.CreatePatientAsync(badTestPatient)).ReturnsAsync(badTestPatient);
            //Act
            Func<Task> result = async () => { await patientProvider.CreatePatientAsync(badTestPatient); };
            //Assert
            await result.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task CreatePatientAsync_WithDuplicateEmail_ThrowsException()
        {
            //Arrange
            var duplicateEmailPatient = new Data.Model.Patient
            {
                FirstName = "Fitzie",
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
            //Act
            Func<Task> result = async () => { await patientProvider.CreatePatientAsync(duplicateEmailPatient); };
            //Assert
            await result.Should().ThrowAsync<ConflictException>();
        }

        [Fact]
        public async Task GetAllPatientsAsync_WithNoPatientDatabase_ThrowsException()
        {
            patientRepositoryStub.Setup(x => x.GetAllPatientsAsync()).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await patientProvider.GetAllPatientsAsync(); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task GetAllPatientsAsync_WithPatientsInDatabase_ReturnsList()
        {
            var result = await patientProvider.GetAllPatientsAsync();
            result.Should().BeEquivalentTo(testPatientDatabase,
                options => options.ComparingByMembers<Data.Model.Patient>());
        }

        [Fact]
        public async Task GetAllMoviesAsync_WithNoMoviesInDatabase_ReturnsEmptyList()
        {
            var emptyTestPatientDatabase = new List<Data.Model.Patient> { };

            patientRepositoryStub.Setup(x => x.GetAllPatientsAsync()).ReturnsAsync(emptyTestPatientDatabase);

            var result = await patientProvider.GetAllPatientsAsync();
            result.Should().BeEquivalentTo(emptyTestPatientDatabase);
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
        public async Task GetPatientByIdAsync_WithValidId_ReturnsPatient()
        {
            var result = await patientProvider.GetPatientByIdAsync(1);
            result.Should().BeEquivalentTo(testPatient1,
                options => options.ComparingByMembers<Data.Model.Patient>());
        }

        [Fact]
        public async Task GetPatientByIdAsync_WithNonexistentId_ThrowsError()
        {
            Func<Task> result = async () => { await patientProvider.GetPatientByIdAsync(100); };
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdatePatientAsync_WithNoDatabase_ThrowsException()
        {
            patientRepositoryStub.Setup(x => x.UpdatePatientAsync(testPatientAltered)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await patientProvider.UpdatePatientAsync(1, testPatientAltered); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task UpdatePatientAsync_WithValidInfo_ReturnsUpdatedDto()
        {
            var result = await patientProvider.UpdatePatientAsync(1, testPatientAltered);
            result.Should().BeEquivalentTo(testPatientAltered,
                options => options.ComparingByMembers<Data.Model.Patient>());

        }

        [Fact]
        public async Task UpdatePatientAsync_WithNonexistentId_ThrowsError()
        {
            Func<Task> result = async () => { await patientProvider.UpdatePatientAsync(100, testPatientAltered); };
            await result.Should().ThrowAsync<NotFoundException>();

        }

        [Fact]
        public async Task UpdatePatientAsync_WithInvalidInfo_ThrowsError()
        {
            var badTestPatientAltered = new Data.Model.Patient
            {
                FirstName = "3",
                LastName = "3",
                Ssn = "1",
                Email = "pridefullongbourne.house",
                Postal = "3",
                Age = 35.1,
                Height = 72.1,
                Weight = -200,
                Gender = "Droid",
            };
            Func<Task> result = async () => { await patientProvider.UpdatePatientAsync(1, badTestPatientAltered); };
            await result.Should().ThrowAsync<BadRequestException>();

        }

        [Fact]
        public async Task UpdatePatientAsync_WithAlteredId_ThrowsError()
        {
            var badTestPatientAltered = new Data.Model.Patient
            {
                Id = 3,
                FirstName = "3",
                LastName = "3",
                Ssn = "1",
                Email = "pridefullongbourne.house",
                Postal = "3",
                Age = 35.1,
                Height = 72.1,
                Weight = -200,
                Gender = "Droid",
            };
            Func<Task> result = async () => { await patientProvider.UpdatePatientAsync(1, badTestPatientAltered); };
            await result.Should().ThrowAsync<BadRequestException>();

        }
        [Fact]
        public async Task UpdatePatientAsync_WithDuplicateEmail_ThrowsException()
        {
            //Arrange
            var duplicateEmailPatient = new Data.Model.Patient
            {
                FirstName = "Fitzie",
                LastName = "Darcy",
                Ssn = "123-12-1234",
                Email = "misunderstood@longbourne.house",
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
            //Act
            Func<Task> result = async () => { await patientProvider.UpdatePatientAsync(1, duplicateEmailPatient); };
            //Assert
            await result.Should().ThrowAsync<ConflictException>();
        }

        [Fact]
        public async Task DeletePatientAsync_WithNoPatientDatabase_ThrowsException()
        {
            patientRepositoryStub.Setup(x => x.DeletePatientByIdAsync(1)).Throws(new ServiceUnavailableException("Oops"));
            Func<Task> result = async () => { await patientProvider.DeletePatientByIdAsync(1); };
            //Assert
            await result.Should().ThrowAsync<ServiceUnavailableException>();

        }

        [Fact]
        public async Task DeletePatientAsync_WithValidId_ReturnsDeletedDto()
        {
            var result = await patientProvider.DeletePatientByIdAsync(3);
            result.Should().BeEquivalentTo(testPatient3,
                options => options.ComparingByMembers<Data.Model.Patient>());

        }

        [Fact]
        public async Task DeletePatientAsync_WithNonexistentId_ThrowsException()
        {
            Func<Task> result = async () => { await patientProvider.DeletePatientByIdAsync(100); };
            await result.Should().ThrowAsync<NotFoundException>();

        }

            [Fact]
        public void ValiditeIfEmptyOrNull_ReturnsTrueIfEmpty()
        {
            var given = string.Empty;
            var actual = patientProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfEmptyOrNull_ReturnsTrueIfNull()
        {
            string given = null;
            var actual = patientProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfEmptyOrNull_ReturnsTrueIfOnlySpaces()
        {
            var given = "   ";
            var actual = patientProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeTrue();
        }

        [Fact]
        public void ValidateIfEmptyOrNull_ReturnsFalseIfNotEmptyOrNull()
        {
            var given = "I have no patients for this...";
            var actual = patientProvider.ValidateIfEmptyOrNull(given);
            actual.Should().BeFalse();
        }
    }
}


