# Super Health Inc. Proof of Concept API

## Description

This is the logic (back-end) portion of a basic healthcare web application that fulfills all the requirements for Catalyte's Final Health Project v5. It allows a user to create, retrieve, update, and delete both patients and patient encounters from a database.

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

Postman is a free application that allows a user to send requests to the database. A Postman collection demonstrating all the functional requirements for this project can be found inside the GitLab repo folder, or by following [this](https://gitlab.ce.catalyte.io/training/cycleworkinggroups/nationwide/associates/emily-huffman/final-project-api/-/blob/main/Health%20API%20Collection.postman_collection.json) link. If you would like to try making your own requests, please download Postman and use localhost:8085/ as your base URL.

### Linting

All files should be appropriately formatted. To lint project files on a Mac, press `^i` simaltaneously.

## Usage

You will find the following methods for patients and encounters.

### Patients

- CreatePatientAsync takes in a JSON-formatted patient object and persists it to the database at the /patients endpoint.
- GetAllPatientsAsync retrieves all patients stored within the /patients endpoint. It doesn't take in any parameters.
- GetPatientByIdAsync retrieves a single patient stored within the /patients endpoint, based on that patient's assigned ID, which is used as a parameter for retrieval; eg. /patients/1
- UpdatePatientAsync updates an existing patient's information using an inputted JSON-formatted patient object. The patient is retrieved from the database using its ID as an endpoint parameter; eg. /patients/1
- DeletePatientByIdAsync removes an existing patient from the database using its ID as an endpoint parameter; eg. /patients/1. A 204 No Content code is returned upon successful deletion.

### Encounters

- CreateEncounterAsync takes in a JSON-formatted encounter object, as well as the ID of the patient the encounter is associated with, and persists the encounter to the database at the patients/{id}/encounters endpoint.
- GetAllEncountersByIdAsync retrieves all encounters associated with a single patient stored within the /patients/{id}/encounters endpoint. It takes in the ID of the patient as a parameter.
- GetEncounterByIdAsync retrieves a single encounter stored within the /patients/{id}/encounters endpoint, based on that encounter's assigned ID, which is used as a parameter for retrieval; eg. /patients/1/encounters/1. The patient ID of the associated patient is also used as a parameter.
- UpdateEncounterAsync updates information associated with an existing encounter using an inputted JSON-formatted encounter object. The encounter is retrieved from the database using its ID and the ID of the patient associated with the encounter as endpoint parameters; eg. patients/1/encounters/1

### JSON Formats

Feel free to submit any and all requests via Postman! A properly-formatted patient object looks like the following:

        {
            "firstName": "Fitzwilliam",
            "lastName": "Darcy",
            "ssn": "122-22-2222",
            "email": "prideful@longbourne.house",
            "street": "Longbourne",
            "city": "Oxford",
            "state": "MS",
            "postal": "11224",
            "age": 35,
            "height": 65,
            "weight": 200,
            "insurance": "ColonialCare",
            "gender": "Male"
        }
 
A properly-formatted encounter object looks like the following:
 
        {
            "patientId": 1,
            "notes": "V broody today",
            "visitCode": "P0R 1D3",
            "provider": "Dr. Phil Collinsworth",
            "billingCode": "111.111.111-11",
            "icd10": "A22",
            "totalCost": 22.99,
            "copay": 1.00,
            "chiefComplaint": "Heartsickness",
            "pulse": 65,
            "systolic": 200,
            "diastolic": 100,
            "date": "2022-02-22"
        }
        
For each object, all fields that are required to be input and/or formatted correctly include validation.

## Testing

Unit test line coverage is currently 98.66%. Integration tests cover all 2XX and 4XX scenarios given in the project requirements. To run tests, select Run on the top menu of Visual Studio and then click on Run Unit Tests. To view coverage reports and coverage gutters on a Mac, follow the instructions [here](https://github.com/ademanuele/VSMac-CodeCoverage) to install and run the VSMac-CodeCoverage extension. Clicking the Gather Coverage button on the window that pulls out to the right side will also run tests.

## Swashbuckle
Also known as Swagger, this tool creates an interface for the API when you run the application.  You can use markup in the controllers to show documentation on the interface.  It is preinstalled with basic configuration (Startup.cs) when creating a new API with the newest versions of Visual Studio.
https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
