using HealthAPI.IntegrationTests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using HealthAPI.DTOs;

namespace HealthAPI.IntegrationTests.IntegrationTests
{
    [Collection("Sequential")]
    public class PatientIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;//Postman in code

        public PatientIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions//initialize the above
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAllPatientsAsync_Returns200()
        {

            var response = await _client.GetAsync("/patients");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]
        public async Task CreatePatientAsync_Returns201()
        {
            var patientDTO = new PatientDTO
            {
                Id = 5,
                FirstName = "Fitzwilliam",
                LastName = "Darcy",
                Ssn = "123-12-1234",
                Email = "manohouse@longbourne.house",
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

            var patientDTOJson = JsonContent.Create(patientDTO);

            var response = await _client.PostAsync("/patients", patientDTOJson);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

        [Fact]
        public async Task CreatePatientAsync_WithDuplicateEmail_Returns409()
        {
            var patientDTO = new PatientDTO
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

            var patientDTOJson = JsonContent.Create(patientDTO);

            var response = await _client.PostAsync("/patients", patientDTOJson);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        }

        [Fact]
        public async Task CreatePatientAsync_WithInvalidInformation_Returns400()
        {
            var patientDTO = new PatientDTO
            {
                Id = 5,
                FirstName = "2",
                LastName = "2",
                Ssn = "1",
                Email = "pridefullongbourne.house",
                Street = "123 Longbourne Circle",
                City = "Longbourne",
                State = "TN",
                Postal = "1",
                Age = 35.2,
                Height = -72,
                Weight = -200,
                Insurance = "Queen's Care",
                Gender = "Muppet",
            };

            var patientDTOJson = JsonContent.Create(patientDTO);

            var response = await _client.PostAsync("/patients", patientDTOJson);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task GetPatientByIdAsync_WithExistingId_Returns200()
        {

            var response = await _client.GetAsync("/patients/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task GetPatientByIdAsync_WithNonexistentId_Returns404()
        {

            var response = await _client.GetAsync("/patients/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
        [Fact]
        public async Task UpdatePatientByIdAsync_Returns200()
        {
            var patientDTO = new PatientDTO
            {
                Id = 10,
                FirstName = "Charlotte",
                LastName = "Collins",
                Ssn = "123-12-1234",
                Email = "patient@longbourne.house",
                Street = "123 Lady Catherine Manor",
                City = "DeBurgh",
                State = "TN",
                Postal = "12321",
                Age = 35,
                Height = 72,
                Weight = 200,
                Insurance = "Queen's Care",
                Gender = "Female",
            };

            var updatedPatientDTO = new PatientDTO
            {
                Id = 10,
                FirstName = "Charlotte",
                LastName = "Collins",
                Ssn = "123-12-1234",
                Email = "patient@longbourne.house",
                Street = "123 Lady Catherine Manor",
                City = "DeBurgh",
                State = "TN",
                Postal = "12321",
                Age = 35,
                Height = 62,
                Weight = 110,
                Insurance = "Queen's Care",
                Gender = "Female",
            };

            var patientDTOJson = JsonContent.Create(patientDTO);
            await _client.PostAsync("/patients", patientDTOJson);
            var existingPatient = await (await _client.GetAsync("/patients/10")).Content.ReadAsAsync<PatientDTO>();
            Assert.Equal("Queen's Care", existingPatient.Insurance);
            var response = await _client.PutAsJsonAsync("/patients/10", updatedPatientDTO);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task UpdatePatientByIdAsync_WithDuplicateEmail_Returns409()
        {
            var patientDTO = new PatientDTO
            {
                Id = 15,
                FirstName = "Lydia",
                LastName = "Bennett-Wickham",
                Ssn = "123-12-1234",
                Email = "thebestbennetdaughter@longbourne.house",
                Street = "123 Elopement Blvd",
                City = "London",
                State = "TN",
                Postal = "12321",
                Age = 16,
                Height = 64,
                Weight = 132,
                Insurance = "Queen's Care",
                Gender = "Female",
            };

            var secondPatientDTO = new PatientDTO
            {
                Id = 16,
                FirstName = "Mister",
                LastName = "Wickham",
                Ssn = "123-12-1234",
                Email = "cad@militia.mil",
                Street = "123 Elopement Blvd",
                City = "London",
                State = "TN",
                Postal = "12321",
                Age = 35,
                Height = 72,
                Weight = 200,
                Insurance = "Queen's Care",
                Gender = "Male",
            };

            var updatedPatientDTO = new PatientDTO
            {
                Id = 15,
                FirstName = "Lydia",
                LastName = "Bennett-Wickham",
                Ssn = "123-12-1234",
                Email = "cad@militia.mil",
                Street = "123 Elopement Blvd",
                City = "London",
                State = "TN",
                Postal = "12321",
                Age = 16,
                Height = 64,
                Weight = 132,
                Insurance = "Queen's Care",
                Gender = "Female",
            };

            var patientDTOJson = JsonContent.Create(patientDTO);
            await _client.PostAsync("/patients", patientDTOJson);
            var existingPatient = await (await _client.GetAsync("/patients/15")).Content.ReadAsAsync<PatientDTO>();
            Assert.Equal("Queen's Care", existingPatient.Insurance);

            var secondPatientDTOJson = JsonContent.Create(secondPatientDTO);
            await _client.PostAsync("/patients", secondPatientDTOJson);
            var secondExistingPatient = await (await _client.GetAsync("/patients/16")).Content.ReadAsAsync<PatientDTO>();
            Assert.Equal("Queen's Care", existingPatient.Insurance);

            var response = await _client.PutAsJsonAsync("/patients/15", updatedPatientDTO);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        }

        [Fact]
        public async Task UpdatePatientByIdAsync_WithInvalidInformation_Returns400()
        {
            var patientDTO = new PatientDTO
            {
                Id = 20,
                FirstName = "Georgiana",
                LastName = "Darcy",
                Ssn = "123-12-1234",
                Email = "sweetiepianolover@longbourne.house",
                Street = "123 Longbourne Circle",
                City = "Longbourne",
                State = "TN",
                Postal = "12321",
                Age = 20,
                Height = 61,
                Weight = 102,
                Insurance = "Queen's Care",
                Gender = "Female",
            };

            var updatedPatientDTO = new PatientDTO
            {
                Id = 20,
                FirstName = "3",
                LastName = "3",
                Ssn = "12",
                Email = "sweets",
                State = "TNT",
                Postal = "1",
                Age = 3.5,
                Height = 7.2,
                Weight = -200,
                Gender = "Tinkerbell",
            };

            var patientDTOJson = JsonContent.Create(patientDTO);
            await _client.PostAsync("/patients", patientDTOJson);
            var existingPatient = await (await _client.GetAsync("/patients/20")).Content.ReadAsAsync<PatientDTO>();
            Assert.Equal("Queen's Care", existingPatient.Insurance);
            var response = await _client.PutAsJsonAsync("/patients/20", updatedPatientDTO);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task UpdatePatientByIdAsync_WithNonexistentId_Returns404()
        {
            var patientDTO = new PatientDTO
            {
                FirstName = "Georgiana",
                LastName = "Darcy",
                Ssn = "123-12-1234",
                Email = "sweetiepianolover@longbourne.house",
                Street = "123 Longbourne Circle",
                City = "Longbourne",
                State = "TN",
                Postal = "12321",
                Age = 20,
                Height = 61,
                Weight = 102,
                Insurance = "Queen's Care",
                Gender = "Female",
            };

            var patientDTOJson = JsonContent.Create(patientDTO);

            var response = await _client.PutAsync("/patients/100", patientDTOJson);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task DeletePatientByIdAsync_WithExistingId_Returns204()
        {

            var response = await _client.DeleteAsync("/patients/3");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

        [Fact]
        public async Task DeletePatientByIdAsync_WithExistingEncounters_Returns409()
        {

            var response = await _client.DeleteAsync("/patients/1");
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        }

        [Fact]
        public async Task DeletePatientByIdAsync_WithNonexistentId_Returns404()
        {

            var response = await _client.DeleteAsync("/patients/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

    }
}