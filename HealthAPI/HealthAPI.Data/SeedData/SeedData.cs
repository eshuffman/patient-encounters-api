//using MovieRentalsAPI.Data.Model;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;

//namespace MovieRentalsAPI.Data.Context
//{
//    public static class Extensions
//    {
//        /// <summary>
//        /// Produces a set of seed data to insert into the database on startup.
//        /// </summary>
//        /// <param name="modelBuilder">Used to build model base DbContext.</param>
//        public static void SeedData(this ModelBuilder modelBuilder)
//        {

//            var movies = new List<Movie>()
//            {
//                new Movie()
//                {
//                    Id = 1,
//                    Sku = "WESAND-2001",
//                    Title = "The Darjeeling Limited",
//                    Genre = "Twee",
//                    Director = "Wes Anderson",
//                    DailyRentalCost = 1.00m
//                },
//                new Movie()
//                {
//                    Id = 2,
//                    Sku = "WESAND-2002",
//                    Title = "The Kolkata Limited",
//                    Genre = "Twee",
//                    Director = "Wes Anderson",
//                    DailyRentalCost = 1.00m
//                },
//                 new Movie()
//                {
//                    Id = 3,
//                    Sku = "WESAND-2003",
//                    Title = "The Delhi Limited",
//                    Genre = "Twee",
//                    Director = "Wes Anderson",
//                    DailyRentalCost = 1.00m
//                }
//            };

//            modelBuilder.Entity<Movie>().HasData(movies);

//            //            var rentedMovies = new List<RentedMovie>()
//            //            {
//            //                new RentedMovie()
//            //                {
//            //                    Id = 1,
//            //                    MovieId = 1,
//            //                    DaysRented = 1
//            //                },
//            //                new RentedMovie()
//            //                {
//            //                    Id = 2,
//            //                    MovieId = 2,
//            //                    DaysRented = 1
//            //                }
//            //            };

//            //            modelBuilder.Entity<RentedMovie>().HasData(rentedMovies);

//            //            var rental = new Rental()
//            //            {
//            //                Id = 1,
//            //                RentalDate = "2022-05-25",
//            //                RentedMovies = rentedMovies,
//            //                RentalTotalCost = 2.00m

//            //            };

//            //            modelBuilder.Entity<Rental>().HasData(rental);
//        }
//    }
//}

