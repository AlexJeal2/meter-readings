ENSEK Technical Test
====================

This project is my submission for the ENSEK Remote Technical Exercise

Running the Project
-------------------

>**Assumptions**  
> - User has Docker installed on thier local machine
> - User has the `dotnet cli` installed on their machine

1. Within the `MeterReadings` directory, run: 

    ```cmd
    docker compose up --build -d
    ```


2. The Web Client should be accessible at `http://localhost:3000`
3. Direct calls to the API can be made at `http://localhost:5000/api/meter-reading-uploads`
4. SQL Server can be accessed with a SQL IDE at `localhost,6000`
5. EF Core database migrations should be done automatically on app start

It should also be possible to run both the API and the WebClient locally using either Visual Studio or the `dotnet cli` using the following commands from the project root:
```cmd
dotnet run --project MeterReadings.API
dotnet run --project MeterReadings.Client
```

Project Cleanup
---------------
```cmd
docker compose down -v
```

Test Coverage
-------------
Unit test coverage for the API is currently ~80%