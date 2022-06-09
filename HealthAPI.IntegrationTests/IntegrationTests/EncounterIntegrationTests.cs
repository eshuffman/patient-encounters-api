//using MovieRentalsAPI.IntegrationTesting.Utilities;
//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using Xunit;
//using MovieRentalsAPI.DTOs;

//namespace MovieRentalsAPI.IntegrationTesting.IntegrationTests
//{
//    [Collection("Sequential")]
//    public class RentalIntegrationTests : IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly HttpClient _client;//Postman in code

//        public RentalIntegrationTests(CustomWebApplicationFactory factory)
//        {
//            _client = factory.CreateClient(new WebApplicationFactoryClientOptions//initialize the above
//            {
//                AllowAutoRedirect = false
//            });
//        }

//        [Fact]
//        public async Task GetAllRentalsAsync_Returns200()
//        {

//            var response = await _client.GetAsync("/rentals");
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//        }

//        [Fact]
//        public async Task CreateRentalAsync_Returns201()
//        {
//            var testRentedMovie = new List<RentedMovieDTO>
//            { new RentedMovieDTO
//            {
//                MovieId = 1,
//                DaysRented = 1
//            }
//            };

//            var rentalDTO = new RentalDTO
//            {
//                RentalDate = "2022-05-20",
//                RentedMovies = testRentedMovie
//            };

//            var rentalDTOJson = JsonContent.Create(rentalDTO);

//            var response = await _client.PostAsync("/rentals", rentalDTOJson);
//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

//        }

//        [Fact]
//        public async Task CreateRentalAsync_WithInvalidInfo_Returns400()
//        {
//            var testRentedMovie = new List<RentedMovieDTO>
//            { new RentedMovieDTO
//            {
//                MovieId = 1,
//                DaysRented = 1
//            }
//            };

//            var rentalDTO = new RentalDTO
//            {
//                RentalDate = "20222-05-20",
//                RentedMovies = testRentedMovie
//            };

//            var rentalDTOJson = JsonContent.Create(rentalDTO);

//            var response = await _client.PostAsync("/rentals", rentalDTOJson);
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

//        }

//        [Fact]
//        public async Task GetRentalByIdAsync_WithExistingId_Returns200()
//        {
//            var testRentedMovie = new List<RentedMovieDTO>
//            { new RentedMovieDTO
//            {
//                MovieId = 1,
//                DaysRented = 1
//            }
//            };

//            var rentalDTO = new RentalDTO
//            {
//                Id = 22,
//                RentalDate = "2022-05-20",
//                RentedMovies = testRentedMovie
//            };

//            var rentalDTOJson = JsonContent.Create(rentalDTO);

//            await _client.PostAsync("/rentals", rentalDTOJson);
//            var existingRental = await (await _client.GetAsync("/rentals/22")).Content.ReadAsAsync<RentalDTO>();
//            Assert.Equal("2022-05-20", existingRental.RentalDate);

//            var response = await _client.GetAsync("/rentals/22");
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//        }

//        [Fact]
//        public async Task GetRentalByIdAsync_WithNonexistentRental_Returns404()
//        {

//            var response = await _client.GetAsync("/rentals/100");
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

//        }

//        [Fact]
//        public async Task UpdateRentalByIdAsync_Returns201()
//        {
//            var testRentedMovie = new List<RentedMovieDTO>
//            { new RentedMovieDTO
//            {
//                MovieId = 1,
//                DaysRented = 1
//            }
//            };

//            var rentalDTO = new RentalDTO
//            {
//                Id = 32,
//                RentalDate = "2022-05-20",
//                RentedMovies = testRentedMovie
//            };

//            var updatedRentalDTO = new RentalDTO
//            {
//                Id = 32,
//                RentalDate = "2022-05-26",
//                RentedMovies = testRentedMovie
//            };

//            var rentalDTOJson = JsonContent.Create(rentalDTO);
//            await _client.PostAsync("/rentals", rentalDTOJson);

//            var existingRental = await (await _client.GetAsync("/rentals/32")).Content.ReadAsAsync<RentalDTO>();
//            Assert.Equal("2022-05-20", existingRental.RentalDate);

//            var response = await _client.PutAsJsonAsync("/rentals/32", updatedRentalDTO);
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//        }

//        [Fact]
//        public async Task UpdateRentalByIdAsync_WithInvalidInformation_Returns400()
//        {
//            var testRentedMovie = new List<RentedMovieDTO>
//            { new RentedMovieDTO
//            {
//                MovieId = 1,
//                DaysRented = 1
//            }
//            };

//            var rentalDTO = new RentalDTO
//            {
//                Id = 28,
//                RentalDate = "2022-05-20",
//                RentedMovies = testRentedMovie
//            };

//            var updatedRentalDTO = new RentalDTO
//            {
//                Id = 28,
//                RentalDate = "20222-05-26",
//                RentedMovies = testRentedMovie
//            };

//            var rentalDTOJson = JsonContent.Create(rentalDTO);
//            await _client.PostAsync("/rentals", rentalDTOJson);

//            var existingRental = await (await _client.GetAsync("/rentals/28")).Content.ReadAsAsync<RentalDTO>();
//            Assert.Equal("2022-05-20", existingRental.RentalDate);

//            var response = await _client.PutAsJsonAsync("/rentals/28", updatedRentalDTO);
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

//        }

//        [Fact]
//        public async Task UpdateRentalByIdAsync_WithNonexistentId_Returns404()
//        {
//            var rentalDTO = new MovieDTO
//            {
//                Sku = "WESAND-2103",
//                Title = "The Goa Limited",
//                Genre = "Twee",
//                Director = "Wes Anderson",
//                DailyRentalCost = 1.99m
//            };

//            var rentalDTOJson = JsonContent.Create(rentalDTO);

//            var response = await _client.PutAsync("/rentals/500", rentalDTOJson);
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

//        }

//        [Fact]
//        public async Task DeleteRentalByIdAsync_WithExistingId_Returns204()
//        {
//            var testRentedMovie = new List<RentedMovieDTO>
//            { new RentedMovieDTO
//            {
//                MovieId = 1,
//                DaysRented = 1
//            }
//            };

//            var rentalDTO = new RentalDTO
//            {
//                Id = 86,
//                RentalDate = "2022-05-20",
//                RentedMovies = testRentedMovie
//            };

//            var rentalDTOJson = JsonContent.Create(rentalDTO);

//            await _client.PostAsync("/rentals", rentalDTOJson);
//            var existingRental = await (await _client.GetAsync("/rentals/86")).Content.ReadAsAsync<RentalDTO>();
//            Assert.Equal("2022-05-20", existingRental.RentalDate);

//            var response = await _client.DeleteAsync("/rentals/86");
//            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

//        }

//        [Fact]
//        public async Task DeleteMovieByIdAsync_WithNonexistentId_Returns404()
//        {

//            var response = await _client.DeleteAsync("/rentals/106");
//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

//        }
//    }
//}