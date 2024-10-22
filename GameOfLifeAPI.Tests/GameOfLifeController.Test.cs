using GameOfLifeAPI.Controllers;
using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Http;

namespace GameOfLifeAPI.Tests.Controllers
{
    public class GameOfLifeControllerTests
    {
        private readonly Mock<ILogger<GameOfLifeController>> _mockLogger;
        private readonly Mock<IGameOfLifeService> _mockGameOfLifeService;
        private readonly GameOfLifeController _controller;

        public GameOfLifeControllerTests()
        {
            _mockLogger = new Mock<ILogger<GameOfLifeController>>();
            _mockGameOfLifeService = new Mock<IGameOfLifeService>();
            _controller = new GameOfLifeController(_mockLogger.Object, _mockGameOfLifeService.Object);
        }

        [Fact]
        public async Task GetNextStateAsync_ReturnsOkResult_WithNextState()
        {
            // Arrange
            var boardId = "test-board-id";
            var nextState = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } };
            _mockGameOfLifeService.Setup(service => service.GetNextStateAsync(boardId)).ReturnsAsync(nextState);

            // Act
            var result = await _controller.GetNextStateAsync(boardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<List<int>>>(okResult.Value);
            Assert.Equal(nextState, returnValue);
        }

        [Fact]
        public async Task GetNextStateAsync_ReturnsBadRequest_OnException()
        {
            // Arrange
            var boardId = "test-board-id";
            _mockGameOfLifeService.Setup(service => service.GetNextStateAsync(boardId)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetNextStateAsync(boardId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public async Task GetFutureStateAsync_ReturnsOkResult_WithFutureState()
        {
            // Arrange
            var boardId = "test-board-id";
            var futureCount = 5;
            var futureState = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } };
            _mockGameOfLifeService.Setup(service => service.GetFurtureStateAsync(boardId, futureCount)).ReturnsAsync(futureState);

            // Act
            var result = await _controller.GetFutureStateAsync(boardId, futureCount);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<List<int>>>(okResult.Value);
            Assert.Equal(futureState, returnValue);
        }

        [Fact]
        public async Task GetFutureStateAsync_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var boardId = "test-board-id";
            var futureCount = 5;
            _mockGameOfLifeService.Setup(service => service.GetFurtureStateAsync(boardId, futureCount)).ThrowsAsync(new ArgumentException("Invalid arguments"));

            // Act
            var result = await _controller.GetFutureStateAsync(boardId, futureCount);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid arguments", badRequestResult.Value);
        }

        [Fact]
        public async Task GetFutureStateAsync_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var boardId = "test-board-id";
            var futureCount = 5;
            _mockGameOfLifeService.Setup(service => service.GetFurtureStateAsync(boardId, futureCount)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetFutureStateAsync(boardId, futureCount);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }

        [Fact]
        public async Task GetFinalStateAsync_ReturnsOkResult_WithFinalState()
        {
            // Arrange
            var boardId = "test-board-id";
            var finalState = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } };
            _mockGameOfLifeService.Setup(service => service.GetFinalStateAsync(boardId)).ReturnsAsync(finalState);

            // Act
            var result = await _controller.GetFinalStateAsync(boardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<List<int>>>(okResult.Value);
            Assert.Equal(finalState, returnValue);
        }

        [Fact]
        public async Task GetFinalStateAsync_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var boardId = "test-board-id";
            _mockGameOfLifeService.Setup(service => service.GetFinalStateAsync(boardId)).ThrowsAsync(new ArgumentException("Invalid arguments"));

            // Act
            var result = await _controller.GetFinalStateAsync(boardId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid arguments", badRequestResult.Value);
        }

        [Fact]
        public async Task GetFinalStateAsync_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var boardId = "test-board-id";
            _mockGameOfLifeService.Setup(service => service.GetFinalStateAsync(boardId)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetFinalStateAsync(boardId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task CreateNewBoardAsync_ReturnsOkResult_WithGameBoardState()
        {
            // Arrange
            var sessionState = new SessionState
            {
                BoardName = "Test Board",
                BoardHeight = 10,
                BoardWidth = 10,
                BoardResolution = 1,
                ShowGrid = true
            };
            var gameBoardState = new GameBoardState
            {
                SessionState = sessionState,
                GameBoardArr = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } }
            };
            _mockGameOfLifeService.Setup(service => service.CreateNewBoard(sessionState)).ReturnsAsync(gameBoardState);

            // Act
            var result = await _controller.CreateNewBoardAsync(sessionState);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GameBoardState>(okResult.Value);
            Assert.Equal(gameBoardState, returnValue);
        }

        [Fact]
        public async Task CreateNewBoardAsync_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var sessionState = new SessionState
            {
                BoardName = "Test Board",
                BoardHeight = 10,
                BoardWidth = 10,
                BoardResolution = 1,
                ShowGrid = true
            };
            _mockGameOfLifeService.Setup(service => service.CreateNewBoard(sessionState)).ThrowsAsync(new ArgumentException("Invalid arguments"));

            // Act
            var result = await _controller.CreateNewBoardAsync(sessionState);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid arguments", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateNewBoardAsync_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var sessionState = new SessionState
            {
                BoardName = "Test Board",
                BoardHeight = 10,
                BoardWidth = 10,
                BoardResolution = 1,
                ShowGrid = true
            };
            _mockGameOfLifeService.Setup(service => service.CreateNewBoard(sessionState)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.CreateNewBoardAsync(sessionState);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }

        [Fact]
        public async Task UpdateBoardState_ReturnsOkResult_WithGameBoardState()
        {
            // Arrange
            var gameBoardState = new GameBoardState
            {
                SessionState = new SessionState
                {
                    BoardName = "Test Board",
                    BoardHeight = 10,
                    BoardWidth = 10,
                    BoardResolution = 1,
                    ShowGrid = true
                },
                GameBoardArr = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } }
            };
            _mockGameOfLifeService.Setup(service => service.UpdateBoardState(gameBoardState)).ReturnsAsync(gameBoardState);

            // Act
            var result = await _controller.UpdateBoardState(gameBoardState);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GameBoardState>(okResult.Value);
            Assert.Equal(gameBoardState, returnValue);
        }

        [Fact]
        public async Task UpdateBoardState_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            var gameBoardState = new GameBoardState
            {
                SessionState = new SessionState
                {
                    BoardName = "Test Board",
                    BoardHeight = 10,
                    BoardWidth = 10,
                    BoardResolution = 1,
                    ShowGrid = true
                },
                GameBoardArr = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } }
            };
            _mockGameOfLifeService.Setup(service => service.UpdateBoardState(gameBoardState)).ThrowsAsync(new ArgumentException("Invalid arguments"));

            // Act
            var result = await _controller.UpdateBoardState(gameBoardState);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid arguments", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateBoardState_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var gameBoardState = new GameBoardState
            {
                SessionState = new SessionState
                {
                    BoardName = "Test Board",
                    BoardHeight = 10,
                    BoardWidth = 10,
                    BoardResolution = 1,
                    ShowGrid = true
                },
                GameBoardArr = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } }
            };
            _mockGameOfLifeService.Setup(service => service.UpdateBoardState(gameBoardState)).ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.UpdateBoardState(gameBoardState);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }

        [Fact]
        public async Task GetStoredBoards_ReturnsOkResult_WithStoredBoards()
        {
            // Arrange
            var storedBoards = new List<GameBoardState>
            {
                new GameBoardState
                {
                    SessionState = new SessionState { BoardName = "Board1", BoardHeight = 10, BoardWidth = 10, BoardResolution = 1, ShowGrid = true },
                    GameBoardArr = new List<List<int>> { new List<int> { 0, 1 }, new List<int> { 1, 0 } }
                }
            };
            _mockGameOfLifeService.Setup(x => x.GetStoredBoards()).ReturnsAsync(storedBoards);

            // Act
            var result = await _controller.GetStoredBoards();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<GameBoardState>>(okResult.Value);
            Assert.Equal(storedBoards.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetStoredBoards_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            _mockGameOfLifeService.Setup(x => x.GetStoredBoards()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetStoredBoards();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}
