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
    /// This class provides the implementation of the IPatientProvider interface, providing service methods for patients.
    /// </summary>
    public class PatientProvider : IPatientProvider
    {
        private readonly ILogger<PatientProvider> _logger;
        private readonly IPatientRepository _patientRepository;
        private readonly IEncounterRepository _encounterRepository;

        public PatientProvider(IPatientRepository patientRepository, IEncounterRepository encounterRepository, ILogger<PatientProvider> logger)
        {
            _logger = logger;
            _patientRepository = patientRepository;
            _encounterRepository = encounterRepository;
        }

        /// <summary>
        /// Persists a new patient to the database.
        /// </summary>
        /// <param name="model">PatientDTO used to build the patient.</param>
        /// <returns>The persisted patient with ID.</returns>
        public async Task<Patient> CreatePatientAsync(Patient newPatient)
        {
            ValidatePatientInputFields(newPatient);

            Patient savedPatient;
            Patient uniquePatient;

            try
            {
                uniquePatient = await _patientRepository.GetPatientByEmailAsync(newPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (uniquePatient != null || uniquePatient != default)
            {
                throw new ConflictException("Email address already exists in the system. Please enter a unique email address.");
            }

            try
            {
                savedPatient = await _patientRepository.CreatePatientAsync(newPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }


            return savedPatient;
        }

        /// <summary>
        /// Retrieves all patients from the database.
        /// </summary>
        /// <returns>All patients as a list of patient objects.</returns>

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            IEnumerable<Patient> patients;

            try
            {
                patients = await _patientRepository.GetAllPatientsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            return patients;
        }

        /// <summary>
        /// Retrieves a single patient from the database via the inputted patient ID parameter
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>Single patient object.</returns>

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            Patient patient;

            try
            {
                patient = await _patientRepository.GetPatientByIdAsync(patientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (patient == null || patient == default)
            {
                _logger.LogInformation($"Patient with id of {patientId} could not be found.");
                throw new NotFoundException($"Patient with id of {patientId} could not be found.");
            }

            return patient;
        }

        /// <summary>
        /// Updates a single patient object based on inputted patient ID, using an updated patient model.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedPatient"></param>
        /// <returns>Updated patient object.</returns>

        public async Task<Patient> UpdatePatientAsync(int id, Patient updatedPatient)
        {
            Patient existingPatient;


            try
            {
                existingPatient = await _patientRepository.GetPatientByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            if (existingPatient == default)
            {
                _logger.LogInformation($"Patient with id of {id} does not exist.");
                throw new NotFoundException($"Patient with id of {id} not found.");
            }

            Patient uniquePatientEmail;

            if (updatedPatient.Email != existingPatient.Email)
            {
                try
                {
                    uniquePatientEmail = await _patientRepository.GetPatientByEmailAsync(updatedPatient);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ServiceUnavailableException("There was a problem connecting to the database.");
                }

                if (uniquePatientEmail != null || uniquePatientEmail != default)
                {
                    throw new ConflictException("Email address already exists in the system. Please enter a unique email address.");
                }
            }

            if (existingPatient.Id != updatedPatient.Id)
            {
                if (updatedPatient.Id == default)
                {
                    updatedPatient.Id = existingPatient.Id;
                }
                else throw new BadRequestException("Patient ID cannot be changed.");
            }

            ValidatePatientInputFields(updatedPatient);

            try
            {
                await _patientRepository.UpdatePatientAsync(updatedPatient);
                _logger.LogInformation("Patient updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            return updatedPatient;
        }

        /// <summary>
        /// Deletes a single patient from the database based on an inputted patient ID.
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns>The deleted patient (no content)</returns>

        public async Task<Patient> DeletePatientByIdAsync(int patientId)
        {
            if (await IsDeleteable(patientId))
            {
                var patient = await _patientRepository.DeletePatientByIdAsync(patientId);
                if (patient == null || patient == default)
                {
                    _logger.LogInformation($"Patient with id: {patientId} could not be found.");
                    throw new NotFoundException($"Patient with id: {patientId} could not be found.");
                }
                return patient;
            }
            throw new ConflictException("Patient has associated encounters and cannot be deleted.");
        }

        ///Validation Methods

        /// <summary>
        /// Validates a patient object's input fields based on various criteria. Returns list of exceptions if any exist.
        /// </summary>
        /// <param name = "newPatient" ></ param >

        public void ValidatePatientInputFields(Patient newPatient)
        {
            List<string> patientExceptions = new();

            if (ValidateIfEmptyOrNull(newPatient.FirstName))
            {
                patientExceptions.Add("Patient first name is required.");
            }
            else if (!ValidateAlphabeticNameInput(newPatient.FirstName))
            {
                patientExceptions.Add("Patient first name must contain only alphabetic characters.");
            }
            if (ValidateIfEmptyOrNull(newPatient.LastName))
            {
                patientExceptions.Add("Patient last name is required.");
            }
            else if (!ValidateAlphabeticNameInput(newPatient.LastName))
            {
                patientExceptions.Add("Patient first name must contain only alphabetic characters.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Ssn))
            {
                patientExceptions.Add("Social security number is required.");
            }
            else if (!ValidateSsnFormat(newPatient.Ssn))
            {
                patientExceptions.Add("SSN must match format xxx-xx-xxxx.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Email))
            {
                patientExceptions.Add("Email is required.");
            }
            else if (!ValidateEmailFormat(newPatient.Email))
            {
                patientExceptions.Add("Email must match format x@x.x.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Street))
            {
                patientExceptions.Add("Street is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.City))
            {
                patientExceptions.Add("City is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.State))
            {
                patientExceptions.Add("State is required.");
            }
            else if (!ValidateStateFormat(newPatient.State))
            {
                patientExceptions.Add("State must be valid two-character state code (eg. AL)");
            }
            if (ValidateIfEmptyOrNull(newPatient.Postal))
            {
                patientExceptions.Add("Postal code is required.");
            }
            else if (!ValidateZipFormat(newPatient.Postal))
            {
                patientExceptions.Add("Postal code must be 5 or 9 digits.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Age.ToString()))
            {
                patientExceptions.Add("Patient age is required.");
            }
            else if (!ValidateWholeInteger(newPatient.Age.ToString()))
            {
                patientExceptions.Add("Age input must only contain numeric digits.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Height.ToString()))
            {
                patientExceptions.Add("Patient height is required.");
            }
            else if (!ValidateWholeInteger(newPatient.Height.ToString()))
            {
                patientExceptions.Add("Height input must only contain numeric digits.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Weight.ToString()))
            {
                patientExceptions.Add("Patient weight is required.");
            }
            else if (!ValidateWholeInteger(newPatient.Weight.ToString()))
            {
                patientExceptions.Add("Weight input must only contain numeric digits.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Insurance))
            {
                patientExceptions.Add("Insurance provider is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Gender))
            {
                patientExceptions.Add("Patient gender is required.");
            }
            else if (!ValidateGenderInput(newPatient.Gender))
            {
                patientExceptions.Add("Patient gender must be 'Male', 'Female', or 'Other'.");
            }

            if (patientExceptions.Count > 0)
            {
                _logger.LogInformation(" ", patientExceptions);
                throw new BadRequestException(string.Join(" ", patientExceptions));
            }
        }

        /// <summary>
        /// Validation method to check if a line of a patient object is empty or null. 
        /// </summary>
        /// <param name="modelField"></param>
        /// <returns>Boolean</returns>
        public bool ValidateIfEmptyOrNull(string modelField)
        {
            return string.IsNullOrWhiteSpace(modelField);
        }

        /// <summary>
        /// Validates if a string only contains alphabetic name characters.
        /// Allows spaces, hyphens, and apostrophes. Used to validate patient first and last names.
        /// </summary>
        /// <param name="modelField">string name input field</param>
        /// <returns>boolean, true if input matches regex</returns>
        public bool ValidateAlphabeticNameInput(string modelField)
        {
            var nameCheck = new Regex(@"^[\p{L} \'\-]+$");
            return nameCheck.IsMatch(modelField);
        }


        /// <summary>
        /// Validates if a social security input matches format DDD-DD-DDDD
        /// </summary>
        /// <param name="modelField">string ssn input field</param>
        /// <returns>boolean, true if input matches regex</returns>
        public bool ValidateSsnFormat(string modelField)
        {
            var ssnCheck = new Regex(@"^[\d]{3}[-][\d]{2}[-][\d]{4}$");
            return ssnCheck.IsMatch(modelField);
        }

        /// <summary>
        /// Validates if user email address is formatted correctly (user@email.com) with alphanumeric username and alphatic domain (EMAIL)
        /// </summary>
        /// <param name="emailAddress">string email input field</param>
        /// <returns>boolean, true if the email is formatted correctly</returns>
        public bool ValidateEmailFormat(string emailAddress)
        {
            var emailCheck = new Regex(@"^\w+@([a-z]+\.)+[a-z]+$");
            return emailCheck.IsMatch(emailAddress);
        }

        /// <summary>
        /// Validates if user state is formatted correctly (LL). Only accepts true US State and territory abbreviations.
        /// </summary>
        /// <param name="state">string email input field</param>
        /// <returns>boolean, true if the email is formatted correctly</returns>
        public bool ValidateStateFormat(string state)
        {
            var stateCheck = new Regex(@"^((A[LKSZR])|(C[AOT])|(D[EC])|(F[ML])|(G[AU])|(HI)|(I[DLNA])|(K[SY])|(LA)|(M[EHDAINSOT])|(N[EVHJMYCD])|(MP)|(O[HKR])|(P[WAR])|(RI)|(S[CD])|(T[NX])|(UT)|(V[TIA])|(W[AVIY]))$");
            return stateCheck.IsMatch(state);
        }

        /// <summary>
        /// Validates if patient postal code is only five or nine numerical digits (ZIPCODE)
        /// </summary>
        /// <param name="zipCode">string zipcode input field</param>
        /// <returns>boolean, true if the zipcode is valid</returns>
        public bool ValidateZipFormat(string zipCode)
        {
            var zipCodeCheck = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");
            return zipCodeCheck.IsMatch(zipCode);
        }

        /// <summary>
        /// Validates if input contains only numeric digits, used to validate patient height, weight, and age.
        /// </summary>
        /// <param name="input">string input field</param>
        /// <returns>boolean, true if the input is valid</returns>
        public bool ValidateWholeInteger(string input)
        {
            var integerCheck = new Regex(@"^\d+$");
            return integerCheck.IsMatch(input);
        }

        /// <summary>
        /// Validates if gender input matches either "Male", "Female", or "Other"
        /// </summary>
        /// <param name="input">string input field</param>
        /// <returns>boolean, true if the input is valid</returns>
        public bool ValidateGenderInput(string input)
        {
            var genderCheck = new Regex(@"^(?:male|Male|female|Female|other|Other)$");
            return genderCheck.IsMatch(input);
        }

        /// <summary>
        /// Checks if patient has associated encounters in database.
        /// </summary>
        /// <param name="patientId">string input field</param>
        /// <returns>boolean, true if no encounters exist</returns>
        public async Task<bool> IsDeleteable(int patientId)
        {
            var encounters = await _encounterRepository.GetAllEncountersByIdAsync(patientId);
            if (encounters.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}