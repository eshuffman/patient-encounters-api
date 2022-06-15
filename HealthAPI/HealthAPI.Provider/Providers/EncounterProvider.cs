using HealthAPI.Data.Interfaces;
using HealthAPI.Data.Model;
using HealthAPI.Provider.Interfaces;
using HealthAPI.Utilities.HttpResponseExceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HealthAPI.Provider.Providers
{
    /// <summary>
    /// This class provides the implementation of the IRentalProvider interface, providing service methods for rentals.
    /// </summary>
    public class EncounterProvider : IEncounterProvider
    {
        private readonly ILogger<EncounterProvider> _logger;
        private readonly IEncounterRepository _encounterRepository;
        private readonly IPatientRepository _patientRepository;

        public EncounterProvider(IEncounterRepository encounterRepository, IPatientRepository patientRepository, ILogger<EncounterProvider> logger)
        {
            _logger = logger;
            _encounterRepository = encounterRepository;
            _patientRepository = patientRepository;
        }

        /// <summary>
        /// Persists a new patient encounter to the database.
        /// </summary>
        /// <param name="newEncounter">Encounter model used to build the encounter object.</param>
        /// <param name="patientId">Id of patient associated with encounter</param>
        /// <returns>The persisted encounter.</returns>
        public async Task<Encounter> CreateEncounterAsync(int patientId, Encounter newEncounter)
        {
            Patient patient;
            ValidateEncounterInputFields(newEncounter, patientId);

            try
            {
                patient = await _patientRepository.GetPatientByIdAsync(patientId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (patient == null)
            {
                throw new NotFoundException($"Patient with ID of {patientId} could not be found in the database.");
            }

            Encounter savedEncounter;

            try
            {
                savedEncounter = await _encounterRepository.CreateEncounterAsync(newEncounter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }


            return savedEncounter;
        }


        /// <summary>
        /// Retrieves all saved patient encounter objects specific to a single patient from database.
        /// </summary>
        /// <param name="patientId">Id of patient associated with encountesr</param>
        /// <returns>List of all patient encounters that match inputted patientId</returns>
        public async Task<IEnumerable<Encounter>> GetAllEncountersByIdAsync(int? patientId)
        {
            List<Encounter> encounters;

            try
            {
                var patient = await _patientRepository.GetPatientByIdAsync(patientId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new NotFoundException($"Patient with ID of {patientId} could not be found in the database.");
            }

            try
            {
                encounters = await _encounterRepository.GetAllEncountersByIdAsync(patientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }
            return encounters;
        }

        /// <summary>
        /// Retrieves a single patient encounter from the database via inputted ID
        /// </summary>
        /// <param name="encounterId"></param>
        /// <returns>Single patient encounter object with matching ID</returns>
        public async Task<Encounter> GetEncounterByIdAsync(int encounterId)
        {
            Encounter existingEncounter;
            try
            {
                existingEncounter = await _encounterRepository.GetEncounterByIdAsync(encounterId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (existingEncounter == null)
            {
                _logger.LogInformation($"Encounter with id of {encounterId} does not exist.");
                throw new NotFoundException($"Encounter with id of {encounterId} not found.");
            }
            return existingEncounter;
        }

        /// <summary>
        /// Updates encounter object in database via inputted encounter ID, using inputted encounter model.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <param name="updatedEncounter"></param>
        /// <returns>Updated rental object.</returns>
        public async Task<Encounter> UpdateEncounterAsync(int encounterId, Encounter updatedEncounter, int patientId)
        {
            Encounter existingEncounter;

            try
            {
                existingEncounter = await _encounterRepository.GetEncounterByIdAsync(encounterId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (existingEncounter == default)
            {
                _logger.LogInformation($"Encounter with id of {encounterId} does not exist.");
                throw new NotFoundException($"Encounter with id of {encounterId} not found.");
            }

            if (updatedEncounter.Id == default)
                updatedEncounter.Id = encounterId;

            ValidateEncounterInputFields(updatedEncounter, patientId);

            try
            {
                await _encounterRepository.UpdateEncounterAsync(updatedEncounter);
                _logger.LogInformation("Encounter updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            return updatedEncounter;
        }
        /// Validation Methods

        /// <summary>
        /// Validation method to check whether patient encounter input fields are not empty and contain appropriately-formatted information
        /// </summary>
        /// <param name="newEncounter"></param>
        public void ValidateEncounterInputFields(Encounter newEncounter, int patientId)
        {
            List<string> encounterExceptions = new();

            if (ValidateIfEmptyOrNull(newEncounter.PatientId.ToString()))
            {
                encounterExceptions.Add("Patient ID is required.");
            }
            else if (newEncounter.PatientId != patientId)
            {
                encounterExceptions.Add("Patient ID must match ID of patient encounter is being recorded for.");
            }
            if (ValidateIfEmptyOrNull(newEncounter.VisitCode))
            {
                encounterExceptions.Add("Visit code is required.");
            }
            else if (!ValidateVisitCodeFormat(newEncounter.VisitCode))
            {
                encounterExceptions.Add("Visit code must match format of LDL DLD (eg. A1A 2B2).");
            }
            if (ValidateIfEmptyOrNull(newEncounter.Provider))
            {
                encounterExceptions.Add("Provider is required.");
            }
            if (ValidateIfEmptyOrNull(newEncounter.BillingCode))
            {
                encounterExceptions.Add("Billing code is required.");
            }
            else if (!ValidateBillingCodeFormat(newEncounter.BillingCode))
            {
                encounterExceptions.Add("Billing code must match format of DDD.DDD.DDD-DD (eg. 123.123.123-12).");
            }
            if (ValidateIfEmptyOrNull(newEncounter.Icd10))
            {
                encounterExceptions.Add("ICD10 code is required.");
            }
            else if (!ValidateIcd10Format(newEncounter.Icd10))
            {
                encounterExceptions.Add("ICD10 must match format of LDD (eg. A12).");
            }
            if (ValidateIfEmptyOrNull(newEncounter.TotalCost.ToString()))
            {
                encounterExceptions.Add("Total cost is required.");
            }
            else if (!ValidatePriceFormat(newEncounter.TotalCost.ToString()))
            {
                encounterExceptions.Add("Total cost must match format of XX.XX.");
            }
            if (ValidateIfEmptyOrNull(newEncounter.Copay.ToString()))
            {
                encounterExceptions.Add("Copay is required.");
            }
            else if (!ValidatePriceFormat(newEncounter.Copay.ToString()))
            {
                encounterExceptions.Add("Copay must match format of XX.XX");
            }
            if (ValidateIfEmptyOrNull(newEncounter.ChiefComplaint))
            {
                encounterExceptions.Add("Chief complaint is required.");
            }
            if (ValidateIfEmptyOrNull(newEncounter.Date))
            {
                encounterExceptions.Add("Encounter date is required.");
            }
            else if (!ValidateDateFormat(newEncounter.Date))
            {
                encounterExceptions.Add("Date must match format of YYYY-MM-DD.");
            }
            if (!ValidateIfEmptyOrNull(newEncounter.Pulse.ToString()))
            {
                if (!ValidateWholeInteger(newEncounter.Pulse.ToString()))
                {
                    encounterExceptions.Add("Pulse must be a whole integer.");
                }
            }
            if (!ValidateIfEmptyOrNull(newEncounter.Systolic.ToString()))
            {
                if (!ValidateWholeInteger(newEncounter.Systolic.ToString()))
                {
                    encounterExceptions.Add("Systolic must be a whole integer.");
                }
            }
            if (!ValidateIfEmptyOrNull(newEncounter.Diastolic.ToString()))
            {
                if (!ValidateWholeInteger(newEncounter.Diastolic.ToString()))
                {
                    encounterExceptions.Add("Diastolic must be a whole integer.");
                }
            }

            if (encounterExceptions.Count > 0)
            {
                _logger.LogInformation(" ", encounterExceptions);
                throw new BadRequestException(string.Join(" ", encounterExceptions));
            }
        }

        //Helper validation method to check whether a string is empty or null.
        public bool ValidateIfEmptyOrNull(string modelField)
        {
            return string.IsNullOrWhiteSpace(modelField);
        }

        /// <summary>
        /// Helper validation method using Regex to verify that a visit code format matches LDL DLD
        /// </summary>
        /// <param name="modelField"></param>
        /// <returns>Boolean</returns>
        public bool ValidateVisitCodeFormat(string modelField)
        {
            Regex visitCodeFormat = new Regex(@"^[A-Z][\d][A-Z][\s][\d][A-Z][\d]$");
            return visitCodeFormat.IsMatch(modelField);
        }

        /// <summary>
        /// Helper validation method using Regex to verify that a billing code format matches DDD.DDD.DDD-DD
        /// </summary>
        /// <param name="modelField"></param>
        /// <returns>Boolean</returns>
        public bool ValidateBillingCodeFormat(string modelField)
        {
            Regex billingCodeFormat = new Regex(@"^[\d]{3}.[\d]{3}.[\d]{3}-[\d]{2}$");
            return billingCodeFormat.IsMatch(modelField);
        }

        /// <summary>
        /// Helper validation method using Regex to verify that ICD10 format matches LDD
        /// </summary>
        /// <param name="modelField"></param>
        /// <returns>Boolean</returns>
        public bool ValidateIcd10Format(string modelField)
        {
            Regex icdTenFormat = new Regex(@"^[A-Z][\d]{2}$");
            return icdTenFormat.IsMatch(modelField);
        }


        /// <summary>
        /// Helper validation method using Regex to verify that a price format matches DD.DD
        /// </summary>
        /// <param name="modelField"></param>
        /// <returns>Boolean</returns>
        public bool ValidatePriceFormat(string modelField)
        {
            Regex priceFormat = new Regex(@"^\d+.\d{2}$");
            return priceFormat.IsMatch(modelField);
        }

        /// <summary>
        /// Validates if input contains only numeric digits, used to validate patient encounter pulse, systolic, and diastolic.
        /// </summary>
        /// <param name="input">string input field</param>
        /// <returns>boolean, true if the input is valid</returns>
        public bool ValidateWholeInteger(string input)
        {
            var integerCheck = new Regex(@"^\d+$");
            return integerCheck.IsMatch(input);
        }

        /// <summary>
        /// Helper validation method using Regex to verify that a date format matches YYYY-MM-DD
        /// </summary>
        /// <param name="modelField"></param>
        /// <returns>Boolean</returns>
        public bool ValidateDateFormat(string modelField)
        {
            Regex dateFormat = new Regex(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$");
            return dateFormat.IsMatch(modelField);
        }
    }
}