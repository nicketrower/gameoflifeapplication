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

## GameOfLifeService Class Overview

The `GameOfLifeService class` in GameOfLifeAPI.Services provides various functionalities related to managing and processing the state of a Game of Life board. Here's a summary of its key functionalities:
Dependencies
•	IRedisCacheService: Used for caching game board states.
•	ILogger: Used for logging errors and information.
Key Functionalities
1.	GetNextStateAsync
•	Retrieves the next state of the game board based on the current state.
•	Throws KeyNotFoundException if the board state is not found in the cache.
•	Throws InvalidOperationException if an error occurs while processing the next state.
2.	GetStoredBoards
•	Retrieves all stored game board states from the cache.
•	Returns an empty list if no states are found.
•	Throws InvalidOperationException if an error occurs while retrieving the stored boards.
3.	GetFurtureStateAsync
•	Retrieves the future state of the game board based on the current state and the number of iterations.
•	Throws KeyNotFoundException if the board state is not found in the cache.
•	Throws InvalidOperationException if an error occurs while processing the future state.
4.	GetFinalStateAsync
•	Retrieves the final state of the game board based on the current state until it reaches a stable state.
•	Throws KeyNotFoundException if the board state is not found in the cache.
•	Throws InvalidOperationException if an error occurs while processing the final state or if the board does not stabilize after 40 iterations.
5.	CreateNewBoard
•	Creates a new game board based on the provided session state.
•	Stores the new board state in the cache.
•	Throws InvalidOperationException if the board state cannot be created in the cache.
6.	UpdateBoardState
•	Updates the game board state in the cache.
•	Throws InvalidOperationException if the board state cannot be updated in the cache.
7.	NextStateIterate
•	Calculates the next state of the game board based on the current state.
•	Updates the board state in the cache.
•	Throws InvalidOperationException if an error occurs while updating the board state in the cache.
8.	AreBoardsEqual
•	Compares two game boards to check if they are equal.
Error Handling
•	The service uses logging to record errors and important information.
•	It throws specific exceptions (KeyNotFoundException, InvalidOperationException) to indicate different types of errors.
Summary
The GameOfLifeService class is responsible for managing the lifecycle of Game of Life boards, including creating new boards, updating states, and retrieving future or final states. It heavily relies on caching to store and retrieve board states and includes robust error handling and logging mechanisms.

## RedisCacheService Class Overview

The `RedisCacheService class` in the Game of Life application is responsible for interacting with the Redis cache to store and retrieve game board states. Here are the key functionalities of the RedisCacheService class:
Key Functionalities
1.	SetCacheValueAsync
•	Purpose: Stores a value in the Redis cache with a specified expiration time.
•	Parameters:
•	string key: The key under which the value is stored.
•	T value: The value to be stored.
•	TimeSpan expiration: The expiration time for the cached value.
•	Returns: Task<bool> indicating whether the operation was successful.
2.	GetCacheValueAsync
•	Purpose: Retrieves a value from the Redis cache based on the provided key.
•	Parameters:
•	string key: The key of the value to be retrieved.
•	Returns: Task<T?> where T is the type of the value. Returns null if the key does not exist.
3.	GetAllKeysAsync
•	Purpose: Retrieves all keys and their corresponding values from the Redis cache.
•	Parameters:
•	Type T: The type of the values to be retrieved.
•	Returns: Task<List<T>> containing all the values stored in the cache.
4.	DeleteCacheValueAsync
•	Purpose: Deletes a value from the Redis cache based on the provided key.
•	Parameters:
•	string key: The key of the value to be deleted.
•	Returns: Task<bool> indicating whether the operation was successful.
Error Handling
•	The service uses logging to record errors and important information.
•	It handles exceptions to ensure that the application can continue to function even if there are issues with the Redis cache.
Summary
The RedisCacheService class is a crucial component for managing the caching of game board states in the Game of Life application. It provides methods to set, get, and delete cached values, as well as retrieve all keys and their values from the cache. The service ensures efficient data retrieval and storage, leveraging Redis for performance and scalability.