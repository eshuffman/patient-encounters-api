//using MovieRentalsAPI.IntegrationTesting.Utilities;
//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using Xunit;
//using MovieRentalsAPI.DTOs;

//namespace MovieRentalsAPI.IntegrationTesting.IntegrationTests
//{
//    [Collection("Sequential")]
//    public class MovieIntegrationTests : IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly HttpClient _client;//Postman in code

//        public MovieIntegrationTests(CustomWebApplicationFactory factory)
//        {
//            _client = factory.CreateClient(new WebApplicationFactoryClientOptions//initialize the above
//            {
//                AllowAutoRedirect = false
//            });
//        }

//        [Fact]
//        public async Task GetAllMoviesAsync_Returns200()
//        {

//            var response = await _client.GetAsync("/movies");
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//        }

//        [Fact]
//        public async Task CreateMovieAsync_Returns201()
//        {
//            var movieDTO = new MovieDTO
//            {
//                Sku = "WESAND-2001",
//                Title = "The Darjeeling Limited",
//                Genre = "Twee",
//                Director = "Wes Anderson",
//                DailyRentalCost = 1.99m
//            };

//            var movieDTOJson = JsonContent.Create(movieDTO);

//            var response = await _client.PostAsync("/movies", movieDTOJson);
//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

//        }

//        [Fact]
//        public async Task CreateMovieAsync_WithDuplicateSku_Returns409()
//        {
//            var movieDTO = new MovieDTO
//            {
//                Sku = "WESAND-2001",
//                Title = "The Darjeeling Limited",
//                Genre = "Twee",
//                Director = "Wes Anderson",
//                DailyRentalCost = 1.99m
//            };

//            var movieDTOJson = JsonContent.Create(movieDTO);

//            var response = await _client.PostAsync("/movies", movieDTOJson);
//            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

//        }

//        [Fact]
//        public async Task CreateMovieAsync_WithInvalidInformation_Returns400()
//        {
//            var movieDTO = new MovieDTO
//            {
//                Sku = "WESAND-20012",
//                Title = "The Darjeeling Limited",
//                Genre = "Twee",
//                Director = "Wes Anderson",
//                DailyRentalCost = 1.992m
//            };

//            var movieDTOJson = JsonContent.Create(movieDTO);

//            var response = await _client.PostAsync("/movies", movieDTOJson);
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

//        }

//        [Fact]
//        public async Task GetMovieByIdAsync_WithExistingId_Returns200()
//        {

//            var response = await _client.GetAsync("/movies/1");
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//        }

//        [Fact]
//        public async Task GetMovieByIdAsync_WithNonexistentId_Returns404()
//        {

//            var response = await _client.GetAsync("/movies/100");
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

//        }
//        [Fact]
//        public async Task UpdateMovieByIdAsync_Returns201()
//        {
//            var movieDTO = new MovieDTO
//            {
//                Id = 5,
//                Sku = "WESAND-2022",
//                Title = "The Karnataka Limited",
//                Genre = "Twee",
//                Director = "Wes Andersson",
//                DailyRentalCost = 1.99m
//            };

//            var updatedMovieDTO = new MovieDTO
//            {
//                Sku = "WESAND-2021",
//                Title = "The Kolkata Limited",
//                Genre = "Twee",
//                Director = "Wes Andersson",
//                DailyRentalCost = 1.99m
//            };

//            var movieDTOJson = JsonContent.Create(movieDTO);
//            await _client.PostAsync("/movies", movieDTOJson);
//            var existingMovie = await (await _client.GetAsync("/movies/5")).Content.ReadAsAsync<MovieDTO>();
//            Assert.Equal("The Karnataka Limited", existingMovie.Title);
//            var response = await _client.PutAsJsonAsync("/movies/5", updatedMovieDTO);
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//        }

//        [Fact]
//        public async Task UpdateMovieByIdAsync_WithDuplicateSku_Returns409()
//        {
//            var movieDTO = new MovieDTO
//            {
//                Id = 5,
//                Sku = "WESAND-2012",
//                Title = "The Karnataka Limited",
//                Genre = "Twee",
//                Director = "Wes Anderson",
//                DailyRentalCost = 1.99m
//            };

//            var secondMovieDTO = new MovieDTO
//            {
//                Id = 22,
//                Sku = "WESAND-2552",
//                Title = "The Hydrabad Limited",
//                Genre = "Twee",
//                Director = "Wes Anderson",
//                DailyRentalCost = 1.99m
//            };

//            var updatedMovieDTO = new MovieDTO
//            {
//                Sku = "WESAND-2012",
//                Title = "The Kolkata Limited",
//                Genre = "Twee",
//                Director = "Wes Andersson",
//                DailyRentalCost = 1.99m
//            };

//            var movieDTOJson = JsonContent.Create(movieDTO);
//            await _client.PostAsync("/movies", movieDTOJson);
//            var existingMovie = await (await _client.GetAsync("/movies/5")).Content.ReadAsAsync<MovieDTO>();
//            Assert.Equal("The Karnataka Limited", existingMovie.Title);

//            var secondMovieDTOJson = JsonContent.Create(secondMovieDTO);
//            await _client.PostAsync("/movies", secondMovieDTOJson);
//            var secondExistingMovie = await (await _client.GetAsync("/movies/22")).Content.ReadAsAsync<MovieDTO>();
//            Assert.Equal("The Hydrabad Limited", secondExistingMovie.Title);

//            var response = await _client.PutAsJsonAsync("/movies/22", updatedMovieDTO);
//            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

//        }

//        [Fact]
//        public async Task UpdateMovieByIdAsync_WithInvalidInformation_Returns400()
//        {
//            var movieDTO = new MovieDTO
//            {
//                Id = 6,
//                Sku = "WESAND-2062",
//                Title = "The Karnataka Limited",
//                Genre = "Twee",
//                Director = "Wes Andersson",
//                DailyRentalCost = 1.99m
//            };

//            var updatedMovieDTO = new MovieDTO
//            {
//                Sku = "WESAND-20221",
//                Title = "The Kolkata Limited",
//                Genre = "Twee",
//                Director = "Wes Andersson",
//                DailyRentalCost = 1.993m
//            };

//            var movieDTOJson = JsonContent.Create(movieDTO);
//            await _client.PostAsync("/movies", movieDTOJson);
//            var existingMovie = await (await _client.GetAsync("/movies/6")).Content.ReadAsAsync<MovieDTO>();
//            Assert.Equal("The Karnataka Limited", existingMovie.Title);
//            var response = await _client.PutAsJsonAsync("/movies/6", updatedMovieDTO);
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

//        }

//        [Fact]
//        public async Task UpdateMovieByIdAsync_WithNonexistentId_Returns404()
//        {
//            var movieDTO = new MovieDTO
//            {
//                Sku = "WESAND-2003",
//                Title = "The Goa Limited",
//                Genre = "Twee",
//                Director = "Wes Anderson",
//                DailyRentalCost = 1.99m
//            };

//            var movieDTOJson = JsonContent.Create(movieDTO);

//            var response = await _client.PutAsync("/movies/100", movieDTOJson);
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

//        }

//        [Fact]
//        public async Task DeleteMovieByIdAsync_WithExistingId_Returns204()
//        {

//            var response = await _client.DeleteAsync("/movies/1");
//            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

//        }

//        [Fact]
//        public async Task DeleteMovieByIdAsync_WithNonexistentId_Returns404()
//        {

//            var response = await _client.DeleteAsync("/movies/1");
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

//        }
//    }
//}