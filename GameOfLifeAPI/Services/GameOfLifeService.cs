using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Models;
using Polly.Caching;
using System.Text.Json;

namespace GameOfLifeAPI.Services
{
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IRedisCacheService _cacheService;
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(10);
        private static readonly string CacheKey = "GameofLifeDataKey";

        public GameOfLifeService(IRedisCacheService cacheService) {
            _cacheService = cacheService;
        }

        public async Task<string> GetFinalStateAsync()
        {
            MockDTO mockDTO = new MockDTO { Id = 1, Description = "Testing", Name = "Test Name", Type = "Test Type"};

            await _cacheService.SetCacheValueAsync(CacheKey,mockDTO, CacheExpiration);
            return "Complete";
        }

        public async Task<MockDTO> GetFurtureStateAsync()
        {
            MockDTO mockDTO = await _cacheService.GetCacheValueAsync<MockDTO>(CacheKey);
           return mockDTO;
        }

        public string GetNextStateAsync()
        {
            throw new NotImplementedException();
        }

        public string PostStateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
