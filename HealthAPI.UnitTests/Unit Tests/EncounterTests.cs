//using MovieRentalsAPI.Data.Interfaces;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using MovieRentalsAPI.Provider.Providers;
//using MovieRentalsAPI.Utilities.HttpResponseExceptions;
//using FluentAssertions;

//namespace MovieRentalsAPI.Testing.UnitTests
//{
//    public class RentalProviderTests
//    {
//        private readonly Mock<IRentalRepository> rentalRepositoryStub;
//        private readonly Mock<IMovieRepository> movieRepositoryStub;
//        private readonly Mock<ILogger<RentalProvider>> loggerStub;
//        private readonly Mock<ILogger<MovieProvider>> movieLoggerStub;
//        private readonly RentalProvider rentalProvider;
//        private readonly MovieProvider movieProvider;

//        private readonly Data.Model.Movie testMovie1;
//        private readonly Data.Model.Movie testMovie2;
//        private readonly Data.Model.Movie testMovie3;
//        private readonly Data.Model.Rental testRental1;
//        private readonly Data.Model.Rental testRental2;
//        private readonly Data.Model.Rental testRental3;
//        private readonly Data.Model.Rental testRental;
//        private readonly Data.Model.Rental testRentalAltered;
//        private readonly Data.Model.Rental badTestRentalAltered;
//        private readonly Data.Model.Rental badTestRental;
//        private readonly Data.Model.Rental badTestRental2;
//        private readonly Data.Model.Rental badTestRental3;
//        private readonly Data.Model.Rental badTestRental4;
//        private readonly List<Data.Model.RentedMovie> testRentedMovies;
//        private readonly List<Data.Model.RentedMovie> badTestRentedMovies;
//        private readonly List<Data.Model.RentedMovie> badTestRentedMovies2;
//        private readonly List<Data.Model.Movie> testMovieDatabase;
//        private readonly List<Data.Model.Rental> testRentalDatabase;


//        public RentalProviderTests()
//        {
//            // Arrange
//            // Create some movies 
//            rentalRepositoryStub = new Mock<IRentalRepository>();
//            movieRepositoryStub = new Mock<IMovieRepository>();
//            loggerStub = new Mock<ILogger<RentalProvider>>();
//            movieLoggerStub = new Mock<ILogger<MovieProvider>>();
//            rentalProvider = new RentalProvider(rentalRepositoryStub.Object, movieRepositoryStub.Object, loggerStub.Object);
//            movieProvider = new MovieProvider(movieRepositoryStub.Object, movieLoggerStub.Object);

//            testMovie1 = new Data.Model.Movie { Id = 1, Sku = "WESAND-2006", Title = "The Assam Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };
//            testMovie2 = new Data.Model.Movie { Id = 2, Sku = "WESAND-2008", Title = "The Kolkata Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };
//            testMovie3 = new Data.Model.Movie { Id = 3, Sku = "WESAND-2009", Title = "The Delhi Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };

//            testMovieDatabase = new List<Data.Model.Movie> { testMovie1, testMovie2, testMovie3 };
//            testRentalDatabase = new List<Data.Model.Rental> { testRental1, testRental2, testRental3 };

//            testRentedMovies = new List<Data.Model.RentedMovie> {
//                new Data.Model.RentedMovie { MovieId = 1, DaysRented = 1 },
//                new Data.Model.RentedMovie { MovieId = 2, DaysRented = 1 }
//            };

//            badTestRentedMovies = new List<Data.Model.RentedMovie> {
//                new Data.Model.RentedMovie { DaysRented = 1, MovieId = 1 },
//                new Data.Model.RentedMovie { DaysRented = -1 },
//                new Data.Model.RentedMovie { }
//            };

//            badTestRentedMovies2 = new List<Data.Model.RentedMovie> {
//                new Data.Model.RentedMovie { MovieId = 100 },
//            };

//            testRental1 = new Data.Model.Rental
//            {
//                RentalDate = "2022-05-25",
//                RentedMovies = testRentedMovies
//            };

//            testRental2 = new Data.Model.Rental
//            {
//                RentalDate = "2022-05-25",
//                RentedMovies = testRentedMovies
//            };

//            testRental3 = new Data.Model.Rental
//            {
//                RentalDate = "2022-05-25",
//                RentedMovies = testRentedMovies
//            };

//            testRental = new Data.Model.Rental
//            {
//                RentalDate = "2022-05-25",
//                RentedMovies = testRentedMovies
//            };

//            testRentalAltered = new Data.Model.Rental
//            {
//                RentalDate = "2000-05-25",
//                RentedMovies = testRentedMovies
//            };

//            badTestRentalAltered = new Data.Model.Rental
//            {
//                RentalDate = "20001-05-25",
//                RentedMovies = testRentedMovies
//            };

//            badTestRental = new Data.Model.Rental
//            {
//                RentalDate = "20222-05-25",
//                RentedMovies = testRentedMovies
//            };

//            badTestRental2 = new Data.Model.Rental
//            {
//                RentalDate = "2022-05-25"
//            };

//            badTestRental3 = new Data.Model.Rental
//            {
//                RentalDate = "2022-05-25",
//                RentedMovies = badTestRentedMovies
//            };

//            badTestRental4 = new Data.Model.Rental
//            {
//                RentalDate = "2022-05-25",
//                RentedMovies = badTestRentedMovies2
//            };

//            rentalRepositoryStub.Setup(x => x.CreateRentalAsync(testRental)).ReturnsAsync(testRental);
//            movieRepositoryStub.Setup(x => x.GetMovieByIdAsync(3)).ReturnsAsync(testMovie1);
//            movieRepositoryStub.Setup(x => x.GetMovieByIdAsync(2)).ReturnsAsync(testMovie2);
//            movieRepositoryStub.Setup(x => x.GetMovieByIdAsync(1)).ReturnsAsync(testMovie3);
//            movieRepositoryStub.Setup(x => x.GetAllMoviesAsync()).ReturnsAsync(testMovieDatabase);
//            rentalRepositoryStub.Setup(x => x.GetAllRentalsAsync()).ReturnsAsync(testRentalDatabase);
//            rentalRepositoryStub.Setup(x => x.GetRentalByIdAsync(1)).ReturnsAsync(testRental1);
//            rentalRepositoryStub.Setup(x => x.UpdateRentalAsync(testRentalAltered)).ReturnsAsync(testRentalAltered);
//            rentalRepositoryStub.Setup(x => x.DeleteRentalByIdAsync(1)).ReturnsAsync(testRental1);

//        }

//        [Fact]
//        public async Task CreateRentalAsync_WithValidInfo_ReturnsDto()
//        {
//            var result = await rentalProvider.CreateRentalAsync(testRental);
//            result.Should().BeEquivalentTo(testRental,
//                options => options.ComparingByMembers<Data.Model.Rental>());

//        }

//        [Fact]
//        public async Task CreateRentalAsync_WithInvalidInfo_ThrowsError()
//        {

//            Func<Task> result = async () => { await rentalProvider.CreateRentalAsync(badTestRental); };
//            //Assert
//            await result.Should().ThrowAsync<BadRequestException>();

//        }

//        [Fact]
//        public async Task CreateRentalAsync_WithDifferentInvalidInfo_ThrowsError()
//        {

//            Func<Task> result = async () => { await rentalProvider.CreateRentalAsync(badTestRental3); };
//            //Assert
//            await result.Should().ThrowAsync<BadRequestException>();

//        }

//        [Fact]
//        public async Task CreateRentalAsync_WithNoMovies_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.CreateRentalAsync(badTestRental2); };
//            //Assert
//            await result.Should().ThrowAsync<BadRequestException>();

//        }

//        [Fact]
//        public async Task GetAllRentalsAsync_WithRentalsInDatabase_ReturnsRentals()
//        {
//            var result = await rentalProvider.GetAllRentalsAsync();
//            result.Should().BeEquivalentTo(testRentalDatabase,
//                options => options.ComparingByMembers<Data.Model.Rental>());
//        }

//        [Fact]
//        public async Task GetAllRentalsAsync_WithNoRentalsInDatabase_ReturnsEmptyList()
//        {

//            var emptyTestRentalDatabase = new List<Data.Model.Rental> { };

//            rentalRepositoryStub.Setup(x => x.GetAllRentalsAsync()).ReturnsAsync(emptyTestRentalDatabase);

//            var result = await rentalProvider.GetAllRentalsAsync();
//            result.Should().BeEquivalentTo(emptyTestRentalDatabase);
//        }

//        [Fact]
//        public async Task GetRentalByIdAsync_WithValidId_ReturnsRental()
//        {
//            var result = await rentalProvider.GetRentalByIdAsync(1);
//            result.Should().BeEquivalentTo(testRental1,
//                options => options.ComparingByMembers<Data.Model.Rental>());
//        }

//        [Fact]
//        public async Task GetRentalByIdAsync_WithNonexistentId_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.GetRentalByIdAsync(100); };
//            await result.Should().ThrowAsync<NotFoundException>();
//        }

//        [Fact]
//        public async Task UpdateRentalAsync_WithValidInfo_ReturnsUpdatedDto()
//        {
//            var result = await rentalProvider.UpdateRentalAsync(1, testRentalAltered);
//            result.Should().BeEquivalentTo(testRentalAltered,
//                options => options.ComparingByMembers<Data.Model.Rental>());

//        }

//        [Fact]
//        public async Task UpdateRentalAsync_WithNonexistentId_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.UpdateRentalAsync(100, testRentalAltered); };
//            await result.Should().ThrowAsync<NotFoundException>();

//        }

//        [Fact]
//        public async Task UpdateMovieAsync_WithInvalidInfo_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.UpdateRentalAsync(1, badTestRentalAltered); };
//            await result.Should().ThrowAsync<BadRequestException>();

//        }

//        [Fact]
//        public async Task DeleteRentalAsync_WithValidId_ReturnsDeletedDto()
//        {
//            var result = await rentalProvider.DeleteRentalByIdAsync(1);
//            result.Should().BeEquivalentTo(testRental1,
//                options => options.ComparingByMembers<Data.Model.Rental>());

//        }

//        [Fact]
//        public async Task DeleteMovieAsync_WithNonexistentId_ThrowsException()
//        {
//            Func<Task> result = async () => { await rentalProvider.DeleteRentalByIdAsync(100); };
//            await result.Should().ThrowAsync<NotFoundException>();

//        }

//        [Fact]
//        public void ValidatePriceFormat_WithProperFormat_ReturnsTrue()
//        {
//            var given = "22.99";
//            var actual = rentalProvider.ValidatePriceFormat(given);
//            actual.Should().BeTrue();

//        }

//        [Fact]
//        public void ValidatePriceFormat_WithInvalidFormat_ReturnsFalse()
//        {
//            var given = "OOPSIE!";
//            var actual = rentalProvider.ValidatePriceFormat(given);
//            actual.Should().BeFalse();

//        }

//        [Fact]
//        public async Task GetMovieByIdAsync_WithNonexistentId_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.GetMovieByIdAsync(100); };
//            await result.Should().ThrowAsync<NotFoundException>();
//        }

//        [Fact]
//        public async Task ValidateRentedMovieIds_WithNoRentedMovies_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.ValidateRentedMovieIds(badTestRental2); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        [Fact]
//        public async Task ValidateRentedMovieIds_WithNoMovieIds_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.ValidateRentedMovieIds(badTestRental3); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        [Fact]
//        public async Task ValidateRentedMovieIds_WithNonexistentMovieIds_ThrowsError()
//        {
//            Func<Task> result = async () => { await rentalProvider.ValidateRentedMovieIds(badTestRental4); };
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        [Fact]
//        public void ValidateIfEmptyOrNull_ReturnsTrueIfEmpty()
//        {
//            var given = string.Empty;
//            var actual = rentalProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeTrue();
//        }

//        [Fact]
//        public void ValidateIfEmptyOrNull_ReturnsTrueIfNull()
//        {
//            string given = null;
//            var actual = rentalProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeTrue();
//        }

//        [Fact]
//        public void ValidateIfEmptyOrNull_ReturnsTrueIfOnlySpaces()
//        {
//            var given = "   ";
//            var actual = rentalProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeTrue();
//        }

//        [Fact]
//        public void ValidateIfEmptyOrNull_ReturnsFalseIfNotEmptyOrNull()
//        {
//            var given = "I love renting movies!";
//            var actual = rentalProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeFalse();
//        }
//    }
//}