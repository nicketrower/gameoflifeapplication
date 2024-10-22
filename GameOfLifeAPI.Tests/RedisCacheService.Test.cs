using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Models;
using GameOfLifeAPI.Services;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace GameOfLifeAPI.Tests.Services
{
    public class RedisCacheServiceTests
    {
        private readonly Mock<IConnectionMultiplexer> _mockConnectionMultiplexer;
        private readonly Mock<IDatabase> _mockDatabase;
        private readonly Mock<ILogger<RedisCacheService>> _mockLogger;
        private readonly RedisCacheService _service;
        private readonly Mock<IServer> _mockServer;


        public RedisCacheServiceTests()
        {
            _mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _mockDatabase = new Mock<IDatabase>();
            _mockServer = new Mock<IServer>();
            _mockLogger = new Mock<ILogger<RedisCacheService>>();

            _mockConnectionMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);

            _service = new RedisCacheService(_mockConnectionMultiplexer.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SetCacheValueAsync_ReturnsTrue_WhenCacheUpdateSucceeds()
        {
            // Arrange
            var key = "testKey";
            var value = new { Name = "Test" };
            var expiration = TimeSpan.FromMinutes(5);
            var json = JsonSerializer.Serialize(value);

            _mockDatabase.Setup(x => x.StringSetAsync(key, json, expiration, It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _service.SetCacheValueAsync(key, value, expiration);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SetCacheValueAsync_ReturnsFalse_WhenCacheUpdateFails()
        {
            // Arrange
            var key = "testKey";
            var value = new { Name = "Test" };
            var expiration = TimeSpan.FromMinutes(5);
            var json = JsonSerializer.Serialize(value);

            _mockDatabase.Setup(x => x.StringSetAsync(key, json, expiration, It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                         .ThrowsAsync(new RedisException("Redis error"));

            // Act
            var result = await _service.SetCacheValueAsync(key, value, expiration);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetCacheValueAsync_ReturnsValue_WhenKeyExists()
        {
            // Arrange
            var key = "testKey";
            var value = new { Name = "Test" };
            var json = JsonSerializer.Serialize(value);

            _mockDatabase.Setup(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()))
                         .ReturnsAsync(json);

            // Act
            var result = await _service.GetCacheValueAsync<object>(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(value.Name, ((JsonElement)result).GetProperty("Name").GetString());
        }

        [Fact]
        public async Task GetCacheValueAsync_ReturnsNull_WhenKeyDoesNotExist()
        {
            // Arrange
            var key = "testKey";

            _mockDatabase.Setup(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()))
                         .ReturnsAsync((RedisValue)RedisValue.Null);

            // Act
            var result = await _service.GetCacheValueAsync<object>(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCacheValueAsync_ReturnsNull_WhenRedisExceptionOccurs()
        {
            // Arrange
            var key = "testKey";

            _mockDatabase.Setup(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()))
                         .ThrowsAsync(new RedisException("Redis error"));

            // Act
            var result = await _service.GetCacheValueAsync<object>(key);

            // Assert
            Assert.Null(result);
        }

       
        [Fact]
        public async Task GetAllKeysAsync_ReturnsDefault_WhenExceptionIsThrown()
        {
            // Arrange
            _mockServer.Setup(x => x.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>())).Throws(new Exception("Test exception"));

            // Act
            var result = await _service.GetAllKeysAsync<GameBoardState>();

            // Assert
            Assert.Null(result);
        }
    }
}
