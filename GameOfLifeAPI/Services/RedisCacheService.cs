using GameOfLifeAPI.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace GameOfLifeAPI.Services
{
    public class RedisCacheService: IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        #region SetCacheValueAsync
        public async Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize(value);
            await db.StringSetAsync(key, json, expiration);
        }
        #endregion

        #region GetCacheValueAsync
        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync(key);
            return json.HasValue ? JsonSerializer.Deserialize<T>(json) : default;
        }
        #endregion
    }
}
