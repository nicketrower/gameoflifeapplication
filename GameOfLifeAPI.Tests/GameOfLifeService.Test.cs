using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Models;
using GameOfLifeAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;


namespace GameOfLifeAPI.Tests.Services
{
    public class GameOfLifeServiceTests
    {
        private readonly Mock<IRedisCacheService> _mockCacheService;
        private readonly Mock<ILogger<GameOfLifeService>> _mockLogger;
        private readonly GameOfLifeService _service;

        public GameOfLifeServiceTests()
        {
            _mockCacheService = new Mock<IRedisCacheService>();
            _mockLogger = new Mock<ILogger<GameOfLifeService>>();
            _service = new GameOfLifeService(_mockCacheService.Object, _mockLogger.Object);
        }

        
        [Fact]
        public async Task GetNextStateAsync_ThrowsKeyNotFoundException_WhenBoardStateNotFound()
        {
            // Arrange
            var boardId = "test-board-id";
            _mockCacheService.Setup(service => service.GetCacheValueAsync<GameBoardState>(boardId))
                             .ReturnsAsync((GameBoardState)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetNextStateAsync(boardId));
            _mockCacheService.Verify(service => service.GetCacheValueAsync<GameBoardState>(boardId), Times.Once);
        }

        [Fact]
        public async Task GetNextStateAsync_ThrowsInvalidOperationException_WhenExceptionOccurs()
        {
            // Arrange
            var boardId = "test-board-id";
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

            _mockCacheService.Setup(service => service.GetCacheValueAsync<GameBoardState>(boardId))
                             .ReturnsAsync(gameBoardState);
            _mockCacheService.Setup(service => service.SetCacheValueAsync(It.IsAny<string>(), It.IsAny<GameBoardState>(), It.IsAny<TimeSpan>()))
                             .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetNextStateAsync(boardId));
            _mockCacheService.Verify(service => service.GetCacheValueAsync<GameBoardState>(boardId), Times.Once);
        }

        [Fact]
        public async Task GetFurtureStateAsync_ThrowsKeyNotFoundException_WhenBoardStateNotFound()
        {
            // Arrange
            var boardId = "test-board-id";
            var futureCount = 5;
            _mockCacheService.Setup(service => service.GetCacheValueAsync<GameBoardState>(boardId))
                             .ReturnsAsync((GameBoardState)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetFurtureStateAsync(boardId, futureCount));
        }

        [Fact]
        public async Task GetFurtureStateAsync_ThrowsInvalidOperationException_WhenExceptionOccurs()
        {
            // Arrange
            var boardId = "test-board-id";
            var futureCount = 5;
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

            _mockCacheService.Setup(service => service.GetCacheValueAsync<GameBoardState>(boardId))
                             .ReturnsAsync(gameBoardState);
            _mockCacheService.Setup(service => service.SetCacheValueAsync(It.IsAny<string>(), It.IsAny<GameBoardState>(), It.IsAny<TimeSpan>()))
                             .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetFurtureStateAsync(boardId, futureCount));
            _mockCacheService.Verify(service => service.GetCacheValueAsync<GameBoardState>(boardId), Times.Once);
        }

       
        [Fact]
        public async Task GetFinalStateAsync_ThrowsKeyNotFoundException_WhenBoardStateNotFound()
        {
            // Arrange
            var boardId = "test-board-id";
            _mockCacheService.Setup(service => service.GetCacheValueAsync<GameBoardState>(boardId))
                             .ReturnsAsync((GameBoardState)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetFinalStateAsync(boardId));
            _mockCacheService.Verify(service => service.GetCacheValueAsync<GameBoardState>(boardId), Times.Once);
        }

        [Fact]
        public async Task GetFinalStateAsync_ThrowsInvalidOperationException_WhenExceptionOccurs()
        {
            // Arrange
            var boardId = "test-board-id";
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

            _mockCacheService.Setup(service => service.GetCacheValueAsync<GameBoardState>(boardId))
                             .ReturnsAsync(gameBoardState);
            _mockCacheService.Setup(service => service.SetCacheValueAsync(It.IsAny<string>(), It.IsAny<GameBoardState>(), It.IsAny<TimeSpan>()))
                             .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetFinalStateAsync(boardId));
            _mockCacheService.Verify(service => service.GetCacheValueAsync<GameBoardState>(boardId), Times.Once);
        }

        [Fact]
        public void AreBoardsEqual_ReturnsTrue_WhenBoardsAreEqual()
        {
            // Arrange
            var board1 = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 0, 1 }
            };

            var board2 = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 0, 1 }
            };

            // Act
            var result = _service.AreBoardsEqual(board1, board2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AreBoardsEqual_ReturnsFalse_WhenBoardsHaveDifferentDimensions()
        {
            // Arrange
            var board1 = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 1, 0 }
            };

            var board2 = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 0, 1 }
            };

            // Act
            var result = _service.AreBoardsEqual(board1, board2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AreBoardsEqual_ReturnsFalse_WhenBoardsHaveDifferentValues()
        {
            // Arrange
            var board1 = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 1, 0 },
                new List<int> { 1, 0, 1 }
            };

            var board2 = new List<List<int>>
            {
                new List<int> { 1, 0, 1 },
                new List<int> { 0, 0, 0 },
                new List<int> { 1, 0, 1 }
            };

            // Act
            var result = _service.AreBoardsEqual(board1, board2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AreBoardsEqual_ReturnsTrue_WhenBothBoardsAreEmpty()
        {
            // Arrange
            var board1 = new List<List<int>>();
            var board2 = new List<List<int>>();

            // Act
            var result = _service.AreBoardsEqual(board1, board2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateBoardState_ReturnsUpdatedState_WhenCacheUpdateSucceeds()
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

            _mockCacheService
                .Setup(x => x.SetCacheValueAsync(gameBoardState.SessionState.BoardId.ToString(), gameBoardState, It.IsAny<TimeSpan>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.UpdateBoardState(gameBoardState);

            // Assert
            Assert.Equal(gameBoardState, result);
        }

        [Fact]
        public async Task UpdateBoardState_ThrowsInvalidOperationException_WhenCacheUpdateFails()
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

            _mockCacheService
                .Setup(x => x.SetCacheValueAsync(gameBoardState.SessionState.BoardId.ToString(), gameBoardState, It.IsAny<TimeSpan>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateBoardState(gameBoardState));
            Assert.Equal("Failed to update the board state in the cache.", exception.Message);
        }

        [Fact]
        public async Task NextStateIterate_UpdatesBoardStateCorrectly()
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

            var initialBoard = new List<List<int>>
            {
                new List<int> { 0, 1, 0 },
                new List<int> { 0, 1, 0 },
                new List<int> { 0, 1, 0 }
            };

            var expectedNextBoard = new List<List<int>>
            {
                new List<int> { 0, 0, 0 },
                new List<int> { 1, 1, 1 },
                new List<int> { 0, 0, 0 }
            };

            var gameBoardState = new GameBoardState
            {
                SessionState = sessionState,
                GameBoardArr = initialBoard
            };

            _mockCacheService
                .Setup(x => x.SetCacheValueAsync(It.IsAny<string>(), It.IsAny<GameBoardState>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.NextStateIterate(gameBoardState);

            // Assert
            Assert.Equal(expectedNextBoard, result);
            _mockCacheService.Verify(x => x.SetCacheValueAsync(sessionState.BoardId.ToString(), It.IsAny<GameBoardState>(), It.IsAny<TimeSpan>()), Times.Once);
        }

        [Fact]
        public async Task NextStateIterate_ThrowsInvalidOperationException_WhenCacheUpdateFails()
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

            var initialBoard = new List<List<int>>
            {
                new List<int> { 0, 1, 0 },
                new List<int> { 0, 1, 0 },
                new List<int> { 0, 1, 0 }
            };

            var gameBoardState = new GameBoardState
            {
                SessionState = sessionState,
                GameBoardArr = initialBoard
            };

            _mockCacheService
                .Setup(x => x.SetCacheValueAsync(It.IsAny<string>(), It.IsAny<GameBoardState>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.NextStateIterate(gameBoardState));
            Assert.Equal("Failed to update the board state in the cache.", exception.Message);
        }
    }
}
