using GameOfLifeAPI.Models;

namespace GameOfLifeAPI.Interfaces
{
    public interface IGameOfLifeService
    {
        public Task<string> GetNextStateAsync();
        public Task<MockDTO> GetFurtureStateAsync();
        public Task<string> GetFinalStateAsync();
        public Task<string> PostStateAsync();
    }
}
