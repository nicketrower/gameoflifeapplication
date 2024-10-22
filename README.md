# Game of Life Application

## Overview

This repository contains the implementation of the Game of Life application, including the API and (opyional)UI components.

## Project Structure

- `GameOfLifeAPI/`: Contains the backend API for the Game of Life application.
  - `Controllers/`: API controllers.
  - `DataLayer/`: Data access layer.
  - `Interfaces/`: Interface definitions.
  - `Models/`: Data models.
  - `appsettings.json`: Configuration settings.
  - `Dockerfile`: Docker configuration for the API.
- `gameoflifeui/`: Contains the frontend UI for the Game of Life application. (optional, still work in progress)

- `GameOfLifeAPI.Tests/`: Contains unit tests for the API.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

## Getting Started

### Running the API

1. Navigate to the `GameOfLifeAPI` directory:
    ```sh
    cd GameOfLifeAPI
    ```

2. Build the API:
    ```sh
    dotnet build
    ```

3. Run the API:
    ```sh
    dotnet run
    ```

The API will be available at `http://localhost:5000`.

### Running the Application with Docker - Redis Cache is required to save state, use Docker Compose to run the entire solution. 

1. Build the Docker images:
    ```sh
    docker-compose build
    ```

2. Start the containers:
    ```sh
    docker-compose up
    ```

The API application will be available at `http://localhost:8181/swagger/index.html`.

The UI application will be available at `http://localhost:4200`.

## Running Tests

1. Navigate to the `GameOfLifeAPI.Tests` directory:
    ```sh
    cd GameOfLifeAPI.Tests
    ```

2. Run the tests:
    ```sh
    dotnet test
    ```

## Additional Information

For more details, refer to the individual project directories and their respective documentation.