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

        public async Task<IEnumerable<Cell>> GetNextStateAsync(IEnumerable<Cell> seed)
        {
            return KillUnderPopulatedCells(seed);
        }

        public async Task<MockDTO> PostStateAsync(MockDTO mockDTO)
        {
            await _cacheService.SetCacheValueAsync(CacheKey, mockDTO, CacheExpiration);
            return mockDTO;
        }

        private static IEnumerable<Cell> KillUnderPopulatedCells(IEnumerable<Cell> seed)
        {
            return Kill(GetUnderPopulatedCells(seed)).Union(seed);
        }

        private static IEnumerable<Cell> GetUnderPopulatedCells(IEnumerable<Cell> cells)
        {
            return cells.Where(cell => cell.Alive && cell.Neighbours < 2);
        }

        private static IEnumerable<Cell> Kill(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells)
            {
                cell.Kill();
                yield return cell;
            }
        }
    }
}
