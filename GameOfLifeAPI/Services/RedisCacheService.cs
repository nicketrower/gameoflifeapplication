using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using StackExchange.Redis;
using System.Text.Json;

namespace GameOfLifeAPI.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        #region SetCacheValueAsync
        /// <summary>
        /// Sets a value in the Redis cache.
        /// </summary>
        /// <typeparam name="T">The type of the value to cache.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The value to cache.</param>
        /// <param name="expiration">The cache expiration time.</param>
        /// <returns>True if the value was set successfully, otherwise false.</returns>
        public async Task<bool> SetCacheValueAsync<T>(string key, T value, TimeSpan expiration)
        {
            try
            {
                var db = _redis.GetDatabase();
                var json = JsonSerializer.Serialize(value);
                await db.StringSetAsync(key, json, expiration);
                return true;
            }
            catch (RedisException ex)
            {
                _logger.LogError(ex, "Redis error occurred while setting cache value for key: {Key}", key);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting cache value for key: {Key}", key);
                return false;
            }
        }
        #endregion

        #region GetCacheValueAsync
        /// <summary>
        /// Gets a value from the Redis cache.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached value, or default if not found.</returns>
        public async Task<T?> GetCacheValueAsync<T>(string key)
        {
            try
            {
                var db = _redis.GetDatabase();
                var json = await db.StringGetAsync(key);
                return json.HasValue ? JsonSerializer.Deserialize<T>(json.ToString()) : default;
            }
            catch (RedisException ex)
            {
                _logger.LogError(ex, "Redis error occurred while getting cache value for key: {Key}", key);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting cache value for key: {Key}", key);
                return default;
            }
        }
        #endregion


        #region GetAllKeysAsync
        /// <summary>
        /// Gets a value from the Redis cache.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached value, or default if not found.</returns>
        public async Task<List<GameBoardState>> GetAllKeysAsync<T>()
        {
            try
            {
                List<GameBoardState> gameBoardStates = new List<GameBoardState>();

                var endpoints = _redis.GetEndPoints();
                var db = _redis.GetServer(endpoints[0]);
                foreach (var key in db.Keys())
                {
                    var value = await GetCacheValueAsync<GameBoardState>(key);
                    gameBoardStates.Add(value);
                    _logger.LogInformation("Key: {Key}", key);
                }

                return gameBoardStates;
            }
            catch (RedisException ex)
            {
                _logger.LogError(ex, "Redis error occurred while getting cache value for key: {Key}");
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting cache value for key: {Key}");
                return default;
            }
        }
        #endregion
    }
}