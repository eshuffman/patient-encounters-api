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
//    public class MovieProviderTests
//    {
//        private readonly Mock<IMovieRepository> movieRepositoryStub;
//        private readonly Mock<ILogger<MovieProvider>> loggerStub;
//        private readonly MovieProvider movieProvider;

//        private readonly Data.Model.Movie testMovie;
//        private readonly Data.Model.Movie testMovie1;
//        private readonly Data.Model.Movie testMovie2;
//        private readonly Data.Model.Movie testMovie3;
//        private readonly Data.Model.Movie badTestMovie;
//        private readonly Data.Model.Movie badTestMovie2;
//        private readonly Data.Model.Movie duplicateSkuMovie;
//        private readonly Data.Model.Movie testMovieAltered;
//        private readonly Data.Model.Movie badTestMovieAltered;


//        private readonly List<Data.Model.Movie> testMovieDatabase;


//        public MovieProviderTests()
//        {
//            // Arrange
//            // Create some movies 
//            movieRepositoryStub = new Mock<IMovieRepository>();
//            loggerStub = new Mock<ILogger<MovieProvider>>();

//            movieProvider = new MovieProvider(movieRepositoryStub.Object, loggerStub.Object);

//            testMovie = new Data.Model.Movie { Id = 5, Sku = "WESAND-2007", Title = "The Darjeeling Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };
//            testMovie1 = new Data.Model.Movie { Id = 1, Sku = "WESAND-2006", Title = "The Assam Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };
//            testMovie2 = new Data.Model.Movie { Id = 2, Sku = "WESAND-2008", Title = "The Kolkata Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };
//            testMovie3 = new Data.Model.Movie { Id = 3, Sku = "WESAND-2009", Title = "The Delhi Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };
//            badTestMovie = new Data.Model.Movie { Sku = "WESAND-20073", DailyRentalCost = 12.959m };
//            badTestMovie2 = new Data.Model.Movie { Title = "The Delhi Limited", Genre = "Twee", Director = "Wes Anderson" };

//            duplicateSkuMovie = new Data.Model.Movie { Sku = "WESAND-2008", Title = "The Bengal Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 1.00m };
//            testMovieAltered = new Data.Model.Movie { Sku = "WESAND-2006", Title = "The Agra Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 10.00m };
//            badTestMovieAltered = new Data.Model.Movie { Sku = "WESAND-20062", Title = "The Agra Limited", Genre = "Twee", Director = "Wes Anderson", DailyRentalCost = 10.002m };


//            testMovieDatabase = new List<Data.Model.Movie> { testMovie1, testMovie2, testMovie3 };
//            movieRepositoryStub.Setup(x => x.CreateMovieAsync(testMovie)).ReturnsAsync(testMovie);
//            movieRepositoryStub.Setup(x => x.CreateMovieAsync(badTestMovie)).ReturnsAsync(badTestMovie);
//            movieRepositoryStub.Setup(x => x.GetAllMoviesAsync()).ReturnsAsync(testMovieDatabase);
//            movieRepositoryStub.Setup(x => x.GetMovieByIdAsync(1)).ReturnsAsync(testMovie1);
//            movieRepositoryStub.Setup(x => x.UpdateMovieAsync(testMovieAltered)).ReturnsAsync(testMovieAltered);
//            movieRepositoryStub.Setup(x => x.DeleteMovieByIdAsync(1)).ReturnsAsync(testMovie1);

//        }

//        [Fact]
//        public async Task CreateMovieAsync_WithNoDatabase_ThrowsException()
//        {
//            movieRepositoryStub.Setup(x => x.CreateMovieAsync(testMovie)).Throws(new ServiceUnavailableException("Oops"));
//            Func<Task> result = async () => { await movieProvider.CreateMovieAsync(testMovie); };
//            //Assert
//            await result.Should().ThrowAsync<ServiceUnavailableException>();

//        }

//        [Fact]
//        public async Task CreateMovieAsync_WithValidInfo_ReturnsDto()
//        {
//            var result = await movieProvider.CreateMovieAsync(testMovie);
//            result.Should().BeEquivalentTo(testMovie,
//                options => options.ComparingByMembers<Data.Model.Movie>());

//        }

//        [Fact]
//        public async Task CreateMovieAsync_WithInvalidInfo_ThrowsException()
//        {
//            //Arrange
//            //Act
//            Func<Task> result = async () => { await movieProvider.CreateMovieAsync(badTestMovie); };
//            //Assert
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        [Fact]
//        public async Task CreateMovieAsync_MissingRentalCostAndSku_ThrowsException()
//        {
//            //Arrange
//            //Act
//            Func<Task> result = async () => { await movieProvider.CreateMovieAsync(badTestMovie2); };
//            //Assert
//            await result.Should().ThrowAsync<BadRequestException>();
//        }

//        [Fact]
//        public async Task CreateMovieAsync_WithDuplicateSku_ThrowsException()
//        {
//            //Arrange
//            //Act
//            Func<Task> result = async () => { await movieProvider.CreateMovieAsync(duplicateSkuMovie); };
//            //Assert
//            await result.Should().ThrowAsync<ConflictException>();
//        }

//        [Fact]
//        public async Task GetAllMoviesAsync_WithMoviesInDatabase_ReturnsList()
//        {
//            var result = await movieProvider.GetAllMoviesAsync();
//            result.Should().BeEquivalentTo(testMovieDatabase,
//                options => options.ComparingByMembers<Data.Model.Movie>());
//        }

//        [Fact]
//        public async Task GetAllMoviesAsync_WithNoMoviesInDatabase_ReturnsEmptyList()
//        {
//            var emptyTestMovieDatabase = new List<Data.Model.Movie> { };

//            movieRepositoryStub.Setup(x => x.GetAllMoviesAsync()).ReturnsAsync(emptyTestMovieDatabase);

//            var result = await movieProvider.GetAllMoviesAsync();
//            result.Should().BeEquivalentTo(emptyTestMovieDatabase);
//        }

//        [Fact]
//        public async Task GetMovieByIdAsync_WithValidId_ReturnsMovie()
//        {
//            var result = await movieProvider.GetMovieByIdAsync(1);
//            result.Should().BeEquivalentTo(testMovie1,
//                options => options.ComparingByMembers<Data.Model.Movie>());
//        }

//        [Fact]
//        public async Task GetMovieByIdAsync_WithNonexistentId_ThrowsError()
//        {
//            Func<Task> result = async () => { await movieProvider.GetMovieByIdAsync(100); };
//            await result.Should().ThrowAsync<NotFoundException>();
//        }

//        [Fact]
//        public async Task UpdateMovieAsync_WithValidInfo_ReturnsUpdatedDto()
//        {
//            var result = await movieProvider.UpdateMovieAsync(1, testMovieAltered);
//            result.Should().BeEquivalentTo(testMovieAltered,
//                options => options.ComparingByMembers<Data.Model.Movie>());

//        }

//        [Fact]
//        public async Task UpdateMovieAsync_WithNonexistentId_ThrowsError()
//        {
//            Func<Task> result = async () => { await movieProvider.UpdateMovieAsync(100, testMovieAltered); };
//            await result.Should().ThrowAsync<NotFoundException>();

//        }

//        [Fact]
//        public async Task UpdateMovieAsync_WithInvalidInfo_ThrowsError()
//        {
//            Func<Task> result = async () => { await movieProvider.UpdateMovieAsync(1, badTestMovieAltered); };
//            await result.Should().ThrowAsync<BadRequestException>();

//        }

//        [Fact]
//        public async Task UpdateMovieAsync_WithDuplicateSku_ThrowsException()
//        {
//            //Arrange
//            //Act
//            Func<Task> result = async () => { await movieProvider.UpdateMovieAsync(1, duplicateSkuMovie); };
//            //Assert
//            await result.Should().ThrowAsync<ConflictException>();
//        }

//        [Fact]
//        public async Task DeleteMovieAsync_WithValidId_ReturnsDeletedDto()
//        {
//            var result = await movieProvider.DeleteMovieByIdAsync(1);
//            result.Should().BeEquivalentTo(testMovie1,
//                options => options.ComparingByMembers<Data.Model.Movie>());

//        }

//        [Fact]
//        public async Task DeleteMovieAsync_WithNonexistentId_ThrowsException()
//        {
//            Func<Task> result = async () => { await movieProvider.DeleteMovieByIdAsync(100); };
//            await result.Should().ThrowAsync<NotFoundException>();

//        }

//        [Fact]
//        public void ValidateSkuFormat_WithProperSku_ReturnsTrue()
//        {
//            var given = "WESAND-2009";
//            var actual = movieProvider.ValidateSkuFormat(given);
//            actual.Should().BeTrue();

//        }

//        [Fact]
//        public void ValidateSkuFormat_WithInvalidSku_ReturnsFalse()
//        {
//            var given = "Movies are neat!";
//            var actual = movieProvider.ValidateSkuFormat(given);
//            actual.Should().BeFalse();

//        }

//        [Fact]
//        public void ValiditeIfEmptyOrNull_ReturnsTrueIfEmpty()
//        {
//            var given = string.Empty;
//            var actual = movieProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeTrue();
//        }

//        [Fact]
//        public void ValidateIfEmptyOrNull_ReturnsTrueIfNull()
//        {
//            string given = null;
//            var actual = movieProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeTrue();
//        }

//        [Fact]
//        public void ValidateIfEmptyOrNull_ReturnsTrueIfOnlySpaces()
//        {
//            var given = "   ";
//            var actual = movieProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeTrue();
//        }

//        [Fact]
//        public void ValidateIfEmptyOrNull_ReturnsFalseIfNotEmptyOrNull()
//        {
//            var given = "Blockbuster isn't dead!";
//            var actual = movieProvider.ValidateIfEmptyOrNull(given);
//            actual.Should().BeFalse();
//        }
//    }
//}

     
