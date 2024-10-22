using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace GameOfLifeAPI.Services
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IRedisCacheService _cacheService;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(10);
        private List<List<int>> _gameBoard;
        private readonly ILogger<GameOfLifeService> _logger;

        public GameOfLifeService(IRedisCacheService cacheService, ILogger<GameOfLifeService> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
            _gameBoard = new List<List<int>>();
        }

        #region GetNextStateAsync
        /// <summary>
        /// Retrieves the next state of the game board based on the current state.
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <returns>A list of lists representing the next state of the game board.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the board state is not found in the cache.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while processing the next state.</exception>
        public async Task<List<List<int>>> GetNextStateAsync(string id)
        {
            // Retrieve the current game board state from the cache
            GameBoardState gameBoardState = await _cacheService.GetCacheValueAsync<GameBoardState>(id);
            if (gameBoardState == null)
            {
                _logger.LogError($"Failed to retrieve the board state from the cache for ID: {id}");
                throw new KeyNotFoundException("Failed to retrieve the board state from the cache.");
            }

            try
            {
                // Calculate the next state of the game board
                var nextBoardIteration = await NextStateIterate(gameBoardState);
                return nextBoardIteration;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the next state of the board.");
                throw new InvalidOperationException("An error occurred while processing the next state.", ex);
            }
        }
        #endregion

        #region GetStoredBoards
        /// <summary>
        /// Retrieves all stored game board states from the cache.
        /// </summary>
        /// <returns>A list of GameBoardState objects representing the stored game boards.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while retrieving the stored boards.</exception>
        public async Task<List<GameBoardState>> GetStoredBoards()
        {
            try
            {
                // Retrieve all stored game board states from the cache
                List<GameBoardState> storedBoards = await _cacheService.GetAllKeysAsync<List<GameBoardState>>();
                if (storedBoards == null)
                {
                    _logger.LogError("Failed to retrieve the board states from the cache.");
                    return new List<GameBoardState>();
                }

                return storedBoards;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the stored board states.");
                throw new InvalidOperationException("An error occurred while retrieving the stored board states.", ex);
            }
        }
        #endregion

        #region GetFurtureStateAsync
        /// <summary>
        /// Retrieves the future state of the game board based on the current state and the number of iterations.
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <param name="futurecount">The number of future states to calculate.</param>
        /// <returns>A list of lists representing the future state of the game board.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the board state is not found in the cache.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while processing the future state.</exception>
        public async Task<List<List<int>>> GetFurtureStateAsync(string id, int futurecount)
        {
            // Retrieve the current game board state from the cache
            GameBoardState gameBoardState = await _cacheService.GetCacheValueAsync<GameBoardState>(id);
            if (gameBoardState == null)
            {
                _logger.LogError($"Failed to retrieve the board state from the cache for ID: {id}");
                throw new KeyNotFoundException("Failed to retrieve the board state from the cache.");
            }

            try
            {
                // Calculate the future state of the game board for the specified number of iterations
                var futureBoardState = gameBoardState.GameBoardArr;
                if (futurecount > 0)
                {
                    for (int i = 0; i < futurecount; i++)
                    {
                        futureBoardState = await NextStateIterate(new GameBoardState { SessionState = gameBoardState.SessionState, GameBoardArr = futureBoardState });
                    }
                }
                return futureBoardState;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the future state of the board.");
                throw new InvalidOperationException("An error occurred while processing the future state.", ex);
            }
        }
        #endregion

        #region GetFinalStateAsync
        /// <summary>
        /// Retrieves the final state of the game board based on the current state.
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <returns>A list of lists representing the final state of the game board.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the board state is not found in the cache.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while processing the final state.</exception>
        public async Task<List<List<int>>> GetFinalStateAsync(string id)
        {
            // Retrieve the current game board state from the cache
            GameBoardState gameBoardState = await _cacheService.GetCacheValueAsync<GameBoardState>(id);
            if (gameBoardState == null)
            {
                _logger.LogError($"Failed to retrieve the board state from the cache for ID: {id}");
                throw new KeyNotFoundException("Failed to retrieve the board state from the cache.");
            }

            try
            {
                // Calculate the final state of the game board until it reaches a stable state
                var currentBoardState = gameBoardState.GameBoardArr;
                var stateCount = 0;
                List<List<int>> nextBoardState;
                do
                {
                    nextBoardState = await NextStateIterate(new GameBoardState { SessionState = gameBoardState.SessionState, GameBoardArr = currentBoardState });
                    if (AreBoardsEqual(currentBoardState, nextBoardState))
                    {
                        break;
                    }
                    currentBoardState = nextBoardState;
                    stateCount++;
                    // If the board state does not stabilize after 40 iterations, break the loop
                    if (stateCount == 40)
                    {
                        _logger.LogError(new Exception(), "An error occurred while calculating the final state of the board.");
                        throw new InvalidOperationException("An error occurred while calculating the final state of the board.");
                    }
                } while (true);

                return nextBoardState;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the final state of the board.");
                throw new InvalidOperationException("An error occurred while processing the final state.", ex);
            }
        }

        /// <summary>
        /// Compares two game boards to check if they are equal.
        /// </summary>
        /// <param name="board1">The first game board.</param>
        /// <param name="board2">The second game board.</param>
        /// <returns>True if the boards are equal, otherwise false.</returns>
        public bool AreBoardsEqual(List<List<int>> board1, List<List<int>> board2)
        {
            if (board1.Count != board2.Count)
            {
                return false;
            }

            for (int i = 0; i < board1.Count; i++)
            {
                if (board1[i].Count != board2[i].Count)
                {
                    return false;
                }

                for (int j = 0; j < board1[i].Count; j++)
                {
                    if (board1[i][j] != board2[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region CreateNewBoard
        /// <summary>
        /// Creates a new game board based on the provided session state.
        /// </summary>
        /// <param name="sessionState">The session state containing board dimensions and resolution.</param>
        /// <returns>The created game board state.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the board state cannot be created in the cache.</exception>
        public async Task<GameBoardState> CreateNewBoard(SessionState sessionState)
        {
            int numOfRows = sessionState.BoardHeight / sessionState.BoardResolution;
            int numOfCols = sessionState.BoardWidth / sessionState.BoardResolution;

            var random = new Random();
            int GenerateRandomCellState() => random.NextDouble() > 0.5 ? 1 : 0;

            var board = new List<List<int>>();
            for (int i = 0; i < numOfRows; i++)
            {
                var row = new List<int>();
                for (int j = 0; j < numOfCols; j++)
                {
                    row.Add(GenerateRandomCellState());
                }
                board.Add(row);
            }

            _gameBoard = board;

            GameBoardState gameBoardState = new GameBoardState { SessionState = sessionState, GameBoardArr = _gameBoard };
            bool storeState = await _cacheService.SetCacheValueAsync(sessionState.BoardId.ToString(), gameBoardState, CacheExpiration);
            if (storeState)
            {
                return gameBoardState;
            }
            else
            {
                _logger.LogError("Failed to create the board state in the cache for Board ID: {BoardId}", sessionState.BoardId);
                throw new InvalidOperationException("Failed to create the board state in the cache.");
            }
        }
        #endregion

        #region UpdateBoardState
        /// <summary>
        /// Updates the game board state in the cache.
        /// </summary>
        /// <param name="gameBoardState">The game board state to update.</param>
        /// <returns>The updated game board state.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the board state cannot be updated in the cache.</exception>
        public async Task<GameBoardState> UpdateBoardState(GameBoardState gameBoardState)
        {
            bool storeState = await _cacheService.SetCacheValueAsync(gameBoardState.SessionState.BoardId.ToString(), gameBoardState, CacheExpiration);
            if (storeState)
            {
                return gameBoardState;
            }
            else
            {
                _logger.LogError("Failed to update the board state in the cache for Board ID: {BoardId}", gameBoardState.SessionState.BoardId);
                throw new InvalidOperationException("Failed to update the board state in the cache.");
            }
        }
        #endregion

        #region NextStateIterate
        /// <summary>
        /// Calculates the next state of the game board based on the current state.
        /// </summary>
        /// <param name="gameBoardState">The current game board state.</param>
        /// <returns>A list of lists representing the next state of the game board.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while updating the board state in the cache.</exception>
        public async Task<List<List<int>>> NextStateIterate(GameBoardState gameBoardState)
        {
            var board = gameBoardState.GameBoardArr;
            var newBoard = new List<List<int>>(board.Count);

            for (int i = 0; i < board.Count; i++)
            {
                var row = new List<int>(board[i].Count);
                for (int j = 0; j < board[i].Count; j++)
                {
                    int aliveNeighbors = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            if (k == 0 && l == 0) continue;

                            int neighborRow = i + k;
                            int neighborCol = j + l;

                            if (neighborRow >= 0 && neighborRow < board.Count && neighborCol >= 0 && neighborCol < board[i].Count)
                            {
                                aliveNeighbors += board[neighborRow][neighborCol];
                            }
                        }
                    }

                    if (board[i][j] == 1)
                    {
                        row.Add(aliveNeighbors < 2 || aliveNeighbors > 3 ? 0 : 1);
                    }
                    else
                    {
                        row.Add(aliveNeighbors == 3 ? 1 : 0);
                    }
                }
                newBoard.Add(row);
            }

            try
            {
                await UpdateBoardState(new GameBoardState { SessionState = gameBoardState.SessionState, GameBoardArr = newBoard });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the board state in the cache.");
                throw new InvalidOperationException("Failed to update the board state in the cache.", ex);
            }

            return newBoard;
        }
        #endregion
    }
}
