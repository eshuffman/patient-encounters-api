using HealthAPI.IntegrationTests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using HealthAPI.DTOs;

namespace HealthAPI.IntegrationTests.IntegrationTests
{
    [Collection("Sequential")]
    public class EncounterIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;//Postman in code

        public EncounterIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions//initialize the above
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAllEncountersByIdAsync_Returns200()
        {

            var response = await _client.GetAsync("/patients/1/encounters");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task CreateEncounterAsync_Returns201()
        {

            var testEncounterDTO = new Data.Model.Encounter
            {
                Id = 10,
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

            var encounterDTOJson = JsonContent.Create(testEncounterDTO);

            var response = await _client.PostAsync("patients/1/encounters", encounterDTOJson);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

        [Fact]
        public async Task CreateEncounterAsync_WithInvalidInfo_Returns400()
        {
            var testEncounterDTO = new Data.Model.Encounter
            {
                Id = 11,
                PatientId = 1,
                VisitCode = "A",
                BillingCode = "11",
                Icd10 = "A1",
                TotalCost = 99,
                Copay = -1,
                Pulse = 12.2,
                Systolic = 12.3,
                Diastolic = 9.0,
                Date = "179-02-21",
            };

            var encounterDTOJson = JsonContent.Create(testEncounterDTO);

            var response = await _client.PostAsync("patients/1/encounters", encounterDTOJson);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task GetEncounterByIdAsync_WithExistingId_Returns200()
        {

            var encounterDTO = new EncounterDTO
            {
                Id = 13,
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

            var encounterDTOJson = JsonContent.Create(encounterDTO);

            await _client.PostAsync("patients/1/encounters", encounterDTOJson);
            var existingEncounter = await (await _client.GetAsync("patients/1/encounters/13")).Content.ReadAsAsync<EncounterDTO>();
            Assert.Equal("1796-02-21", existingEncounter.Date);

            var response = await _client.GetAsync("patients/1/encounters/13");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task GetEncounterByIdAsync_WithNonexistentEncounter_Returns404()
        {

            var response = await _client.GetAsync("patients/1/encounters/100");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task UpdateEncounterByIdAsync_Returns201()
        {

            var encounterDTO = new EncounterDTO
            {
                Id = 14,
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

            var updatedEncounterDTO = new EncounterDTO
            {
                Id = 14,
                PatientId = 1,
                Notes = "Patient is unreasonably belligerant today",
                VisitCode = "A1A 2B2",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "111.222.333-44",
                Icd10 = "A11",
                TotalCost = 99.99m,
                Copay = 20.01m,
                ChiefComplaint = "Says he's been wronged",
                Pulse = 122,
                Systolic = 123,
                Diastolic = 90,
                Date = "1796-02-21",
            };

            var encounterDTOJson = JsonContent.Create(encounterDTO);
            await _client.PostAsync("patients/1/encounters", encounterDTOJson);

            var existingEncounter = await (await _client.GetAsync("patients/1/encounters/14")).Content.ReadAsAsync<EncounterDTO>();
            Assert.Equal("1796-02-21", existingEncounter.Date);

            var response = await _client.PutAsJsonAsync("patients/1/encounters/14", updatedEncounterDTO);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateEncounterByIdAsync_WithInvalidInformation_Returns400()
        {

            var encounterDTO = new EncounterDTO
            {
                Id = 15,
                PatientId = 1,
                Notes = "Patient is unreasonably belligerant today",
                VisitCode = "A1A 2B2",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "111.222.333-44",
                Icd10 = "A11",
                TotalCost = 99.99m,
                Copay = 20.01m,
                ChiefComplaint = "Says he's been wronged",
                Pulse = 122,
                Systolic = 123,
                Diastolic = 90,
                Date = "1796-02-21",
            };

            var updatedEncounterDTO = new EncounterDTO
            {
                Id = 18,
                PatientId = 1,
                Notes = "Patient is unreasonably belligerant today",
                VisitCode = "A1",
                BillingCode = "1",
                Icd10 = "A",
                TotalCost = 99,
                Copay = -1,
                Pulse = 12.2,
                Systolic = 12.3,
                Diastolic = 90.0,
                Date = "1796-02-2",
            };

            var encounterDTOJson = JsonContent.Create(encounterDTO);
            await _client.PostAsync("patients/1/encounters", encounterDTOJson);

            var existingEncounter = await (await _client.GetAsync("patients/1/encounters/15")).Content.ReadAsAsync<EncounterDTO>();
            Assert.Equal("1796-02-21", existingEncounter.Date);

            var response = await _client.PutAsJsonAsync("patients/1/encounters/15", updatedEncounterDTO);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task UpdateEncounterByIdAsync_WithNonexistentId_Returns404()
        {
            var encounterDTO = new EncounterDTO
            {
                Id = 16,
                PatientId = 1,
                Notes = "Patient is unreasonably belligerant today",
                VisitCode = "A1A 2B2",
                Provider = "Dr. Phil Collinsworth",
                BillingCode = "111.222.333-44",
                Icd10 = "A11",
                TotalCost = 99.99m,
                Copay = 20.01m,
                ChiefComplaint = "Says he's been wronged",
                Pulse = 122,
                Systolic = 123,
                Diastolic = 90,
                Date = "1796-02-21",
            };

            var encounterDTOJson = JsonContent.Create(encounterDTO);

            var response = await _client.PutAsync("/encounters/500", encounterDTOJson);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }
    }
}