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
        public async Task<Encounter> CreateEncounterAsync(int? patientId, Encounter newEncounter)
        {

            //ValidateEncounterInputFields(newEncounter);

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
                _logger.LogInformation($"NotFoundException in WishlistProvider.GetWishlistByIdAsync: {ex.Message}");
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
        /// Updates encounter object in database via inputted encounter ID, using inputted encounter model.
        /// </summary>
        /// <param name="encounterId"></param>
        /// <param name="updatedEncounter"></param>
        /// <returns>Updated rental object.</returns>
        public async Task<Encounter> UpdateEncounterAsync(int encounterId, Encounter updatedEncounter)
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

            //ValidateRentalInputFields(updatedRental);

            try
            {
                await _encounterRepository.UpdateEncounterAsync(updatedEncounter);
                _logger.LogInformation("Rental updated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ServiceUnavailableException("There was a problem connecting to the database.");
            }

            return updatedEncounter;
        }
        ///// Validation Methods

        /////Validation method to check whether a rented movie exists in the database and the inputted value is not empty or null.
        //public async Task<Rental> ValidateRentedMovieIds(Rental newRental)
        //{
        //    var rentedMovies = newRental.RentedMovies;
        //    var invalidIds = new List<int?>();
        //    bool isNotMovie;

        //    if (rentedMovies == null)
        //    {
        //        throw new BadRequestException("Movie ID cannot be empty.");
        //    }

        //    foreach (var rental in rentedMovies)
        //    {
        //        if (rental.MovieId < 0 || rental.MovieId == null)
        //        {
        //            throw new BadRequestException("Movie ID cannot be empty.");
        //        }

        //        try
        //        {
        //            isNotMovie = await ValidateRentedMovieIds(rental);
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex.Message);
        //            throw new ServiceUnavailableException("There was a problem connecting to the database.");
        //        }

        //        if (isNotMovie)
        //        {
        //            invalidIds.Add(rental.MovieId);
        //        }
        //    }

        //    if (invalidIds.Count > 0)
        //    {
        //        _logger.LogInformation("One or more requested movies do not exist in our database.");
        //        throw new BadRequestException("One or more requested movies do not exist in our database.");
        //    }

        //    return newRental;
        //}

        ///// <summary>
        ///// Validation method to check whether a rented movie's input fields are not empty and contain appropriately-formatted information
        ///// </summary>
        ///// <param name="newRental"></param>
        //public void ValidateRentedMovieInputFields(Rental newRental)
        //{
        //    List<string> rentalExceptions = new();
        //    var rentedMovies = newRental.RentedMovies;

        //    foreach (var rental in rentedMovies)
        //    {
        //        string movieId = rental.MovieId.ToString();
        //        string daysRented = rental.DaysRented.ToString();

        //        if (ValidateIfEmptyOrNull(movieId))
        //        {
        //            rentalExceptions.Add("Movie ID is required.");
        //        }
        //        if (ValidateIfEmptyOrNull(daysRented))
        //        {
        //            rentalExceptions.Add("Days rented is required.");
        //        }
        //        if (rental.DaysRented < 0 || rental.DaysRented == null)
        //        {
        //            rentalExceptions.Add("Number of days rented must be greater than zero.");
        //        }

        //        if (rentalExceptions.Count > 0)
        //        {
        //            _logger.LogInformation(" ", rentalExceptions);
        //            throw new BadRequestException(string.Join(" ", rentalExceptions));
        //        }
        //    }
        //}

        ///// <summary>
        ///// Validation method to check whether all fields of a rental object are appropriately formatted an are not empty or null.
        ///// </summary>
        ///// <param name="newRental"></param>
        //public void ValidateRentalInputFields(Rental newRental)
        //{
        //    List<string> rentalExceptions = new();

        //    if (ValidateIfEmptyOrNull(newRental.RentalDate))
        //    {
        //        rentalExceptions.Add("Rental date is required.");
        //    }
        //    else
        //    {
        //        if (!ValidateDateFormat(newRental.RentalDate))
        //            rentalExceptions.Add("Rental date format must match YYYY-MM-DD");
        //    }
        //    if (newRental.RentedMovies == null || newRental.RentedMovies.Count < 1)
        //    {
        //        rentalExceptions.Add("One or more movies must be rented.");
        //    }

        //    if (rentalExceptions.Count > 0)
        //    {
        //        _logger.LogInformation(" ", rentalExceptions);
        //        throw new BadRequestException(string.Join(" ", rentalExceptions));
        //    }
        //}

        ////Helper validation method to check whether a string is empty or null.
        //public bool ValidateIfEmptyOrNull(string modelField)
        //{
        //    return string.IsNullOrWhiteSpace(modelField);
        //}

        /////Helper validation method to check whether movies in a rented movie object exist in the database.
        //public async Task<bool> ValidateRentedMovieIds(RentedMovie addedRental)
        //{
        //    var movieList = await GetAllMoviesAsync();
        //    var movieIdList = new List<int?>();
        //    foreach (var movie in movieList)
        //    {
        //        movieIdList.Add(movie.Id);
        //    }

        //    if (movieIdList.Contains(addedRental.MovieId))
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// Helper method to calculate the total rental price of a rental, based on the individual movies' rental costs and days rented
        ///// (I'm pretty proud of it)
        ///// </summary>
        ///// <param name="newRental"></param>
        ///// <returns>Total cost of rental</returns>
        //public async Task<decimal?> RentedMoviePrice(Rental newRental)
        //{
        //    var rentedMovies = newRental.RentedMovies;
        //    decimal? movieRentalCost = 0.00m;
        //    foreach (var movie in rentedMovies)
        //    {
        //        Movie foundMovie = await GetMovieByIdAsync(movie.MovieId);
        //        decimal? totalIndividualPrice = (foundMovie.DailyRentalCost * movie.DaysRented);
        //        movieRentalCost += totalIndividualPrice;
        //    }
        //    return movieRentalCost;
        //}

        ///// <summary>
        ///// Helper method to pull all movies from the database for use in various validation methods.
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        //{
        //    IEnumerable<Movie> movies;

        //    try
        //    {
        //        movies = await _movieRepository.GetAllMoviesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        throw new ServiceUnavailableException("There was a problem connecting to the database.");
        //    }

        //    return movies;
        //}

        ///// <summary>
        ///// Helper validation method using Regex to verify that a price format matches DD.DD
        ///// </summary>
        ///// <param name="modelField"></param>
        ///// <returns>Boolean</returns>
        //public bool ValidatePriceFormat(string modelField)
        //{
        //    Regex priceFormat = new Regex(@"\d+\.\d\d(?!\d)");
        //    return priceFormat.IsMatch(modelField);
        //}

        ///// <summary>
        ///// Helper validation method using Regex to verify that a date format matches YYYY-MM-DD
        ///// </summary>
        ///// <param name="modelField"></param>
        ///// <returns>Boolean</returns>
        //public bool ValidateDateFormat(string modelField)
        //{
        //    Regex dateFormat = new Regex(@"^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$");
        //    return dateFormat.IsMatch(modelField);
        //}
    }
}