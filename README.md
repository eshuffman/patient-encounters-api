# Movie Rentals Practice API

## Description

This is a practice project in preparation for Catalyte's Software Development final project. It allows a user to create, retrieve, update, and delete both movies and rentals from a database.

## Getting Started/Prerequisites

### Start the Server (Mac-Specific Instructions)

- Press the triangle play button to the right of the red/yellow/green buttons at the top left of the Visual Studio window.

### Connections

By default, this service starts up on port 8085 and accepts cross-origin requests from `*`.

### .Net Runtime

You must have a .Net v5.0 runtime installed on your machine.

### Postgres

This server requires that you have Postgres installed and running on the default Postgres port of 5432. It requires that you have a database created on the server with the name of `postgres`
- Your username should be `postgres`
- Your password should be `root`

### Postman

Postman is a free application that allows a user to send requests to the database. A Postman collection demonstrating all the functional requirements for this project can be found inside the GitLab repo folder, or by following [this](https://gitlab.ce.catalyte.io/training/cycleworkinggroups/nationwide/associates/emily-huffman/movie-rentals-api-practice-project/-/blob/movies-and-rentals/Movie%20Rental%20API%20Collection.postman_collection.json) link. If you would like to try making your own requests, please download Postman and use localhost:8085/ as your base URL.

### Linting

All files should be appropriately formatted. To lint project files on a Mac, press `^i` simaltaneously.

## Usage

You will find five main methods for both movies and rentals.

### Movies

- CreateMovieAsync takes in a JSON-formatted movie object and persists it to the database at the /movies endpoint.
- GetAllMoviesAsync retrieves all movies stored within the /movies endpoint. It doesn't take in any parameters.
- GetMovieByIdAsync retrieves a single movie stored within the /movies endpoint, based on that movie's assigned ID, which is used as a parameter for retrieval; eg. /movies/1
- UpdateMovieAsync updates an existing movie's information using an inputted JSON-formatted movie object. The movie is retrieved from the database using its ID as an endpoint parameter; eg. /movies/1
- DeleteMovieByIdAsync removes an existing movie from the database using its ID as an endpoint parameter; eg. /movies/1. A 204 No Content code is returned upon successful deletion.

### Rentals

- CreateRental async takes in a JSON-formatted rental object, which includes one or more rented movies objects, and persists it to the database at the /rentals endpoint.
- GetAllRentalsAsync retrieves all rentals stored within the /rentals endpoint. It doesn't take in any parameters.
- GetRentalByIdAsync retrieves a single movie stored within the /rentals endpoint, based on that rental's assigned ID, which is used as a parameter for retrieval; eg. /rentals/1
- UpdateRentalAsync updates information associated with an existing rental and/or the rented movie/s inside it using an inputted JSON-formatted rental object. The rental is retrieved from the database using its ID as an endpoint parameter; eg. /rentals/1
- DeleteRentalByIdAsync removes an existing rental from the database using its ID as an endpoint parameter; eg. /rentals/1. A 204 No Content code is returned upon successful deletion.

### JSON Formats

Feel free to submit any and all requests via Postman! A properly-formatted movie object looks like the following:

        {
            "sku": "WESAND-2007",
            "title": "The Darjeeling Limited",
            "genre": "Comedy",
            "director": "Wes Anderson",
            "dailyRentalCost": 1.50
        }
 
A properly-formatted rental object, which includes one or more rented movie objects, looks like the following:
 
        {
            "rentaldate": "2022-05-25",
            "rentedMovies": [
                {
                    "movieId": 1,
                    "daysRented": 12
                },
                {
                    "movieId": 2,
                    "daysRented": 2
                }
            ]
        }
        
Please note, "rentalTotalCost" is a field that is auto-populated with the correct cost of the rental (based on the number of days each movie is rented and how much each movie's daily rental cost is) upon successful persistence to the database. 

For each object, all fields are required and include validation.

## Testing

Unit test line coverage is currently 83.01%. To run tests, select Run on the top menu of Visual Studio and then click on Run Unit Tests. To view coverage reports and coverage gutters on a Mac, follow the instructions [here] (https://github.com/ademanuele/VSMac-CodeCoverage) to install and run the VSMac-CodeCoverage extension. Clicking the Gather Coverage button on the window that pulls out to the right side will also run tests.

## Swashbuckle
Also known as Swagger, this tool creates an interface for the API when you run the application.  You can use markup in the controllers to show documentation on the interface.  It is preinstalled with basic configuration (Startup.cs) when creating a new API with the newest versions of Visual Studio.
https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
