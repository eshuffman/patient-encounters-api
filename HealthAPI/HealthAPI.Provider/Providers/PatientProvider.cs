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

        public PatientProvider(IPatientRepository patientRepository, ILogger<PatientProvider> logger)
        {
            _logger = logger;
            _patientRepository = patientRepository;
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

            // Movie email validation to repository
            //if (await ValidateSingleSku(newMovie))
            //{
            //    _logger.LogError("SKU is already in use. Please enter new SKU.");
            //    throw new ConflictException("SKU is already in use. Please enter new SKU.");
            //}

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

            // MOVE EMAIL VALIDATION TO REPOSITORY
            //if (updatedMovie.Sku != existingMovie.Sku)
            //{
            //    if (await ValidateSingleSku(updatedMovie))

            //    {
            //        _logger.LogError("SKU is already in use. Please enter new SKU.");
            //        throw new ConflictException("SKU is already in use. Please enter new SKU.");
            //    }
            //}

            if (updatedPatient.Id == default)
                updatedPatient.Id = id;

            ValidatePatientInputFields(updatedPatient);

            try
            {
                await _patientRepository.UpdatePatientAsync(updatedPatient);
                _logger.LogInformation("Movie updated.");
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

            var patient = await _patientRepository.DeletePatientByIdAsync(patientId);
            
            if (patient == null || patient == default)
            {
                _logger.LogInformation($"Patient with id: {patientId} could not be found.");
                throw new NotFoundException($"Patient with id: {patientId} could not be found.");
            }
            return patient;
        }

        ///Validation Methods

        /// <summary>
        /// Validates a movie object's input fields based on various criteria. Returns list of exceptions if any exist.
        /// </summary>
        /// <param name = "newMovie" ></ param >

        public void ValidatePatientInputFields(Patient newPatient)
        {
            List<string> patientExceptions = new();

            if (ValidateIfEmptyOrNull(newPatient.FirstName))
            {
                patientExceptions.Add("Patient first name is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.LastName))
            {
                patientExceptions.Add("Patient last name is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Ssn))
            {
                patientExceptions.Add("Social security number is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Email))
            {
                patientExceptions.Add("Email is required.");
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
            if (ValidateIfEmptyOrNull(newPatient.Postal))
            {
                patientExceptions.Add("Postal code is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Age.ToString()))
            {
                patientExceptions.Add("Patient age is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Height.ToString()))
            {
                patientExceptions.Add("Patient height is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Weight.ToString()))
            {
                patientExceptions.Add("Pateint weight is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Insurance))
            {
                patientExceptions.Add("Insurance provider is required.");
            }
            if (ValidateIfEmptyOrNull(newPatient.Gender))
            {
                patientExceptions.Add("Patient gender is required.");
            }
            //else
            //{
            //    if (!ValidatePriceFormat(newMovie.DailyRentalCost.ToString()))
            //        movieExceptions.Add("Price value must match the format DD.DD, where D is any number.");

            //}



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

        ///// <summary>
        ///// Validatation method to check if the price format of a movie object is correctly formated.
        ///// </summary>
        ///// <param name="modelField"></param>
        ///// <returns>Boolean</returns>
        //public bool ValidatePriceFormat(string modelField)
        //{
        //    Regex priceFormat = new Regex(@"\d+\.\d\d(?!\d)");
        //    return priceFormat.IsMatch(modelField);
        //}

        ///// <summary>
        ///// Validation method to check if the SKU format of a movie object is correctly formatted
        ///// </summary>
        ///// <param name="modelField"></param>
        ///// <returns>Boolean</returns>
        //public bool ValidateSkuFormat(string modelField)
        //{
        //    Regex skuFormat = new Regex(@"^[A-Z]{6}[-][0-9]{4}$");
        //    return skuFormat.IsMatch(modelField);

        //}

        ///// <summary>
        ///// Validation method to check if the SKU a movie object is currently in use
        ///// </summary>
        ///// <param name="addedMovie"></param>
        ///// <returns>Boolean</returns>
        //public async Task<bool> ValidateSingleSku(Movie addedMovie)
        //{
        //    var movieList = await GetAllMoviesAsync();
        //    var movieSkuList = new List<string>();
        //    foreach (var movie in movieList)
        //    {
        //        movieSkuList.Add(movie.Sku);
        //    }

        //    if (movieSkuList.Contains(addedMovie.Sku))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

    }

}