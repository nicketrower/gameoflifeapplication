using GameOfLifeAPI.Models;

namespace GameOfLifeAPI.Interfaces
{
    public interface IGameOfLifeService
    {
        public Task<IEnumerable<Cell>> GetNextStateAsync(IEnumerable<Cell> seed);
        public Task<MockDTO> GetFurtureStateAsync();
        public Task<string> GetFinalStateAsync();
        public Task<MockDTO> PostStateAsync(MockDTO mockDTO);
    }
}
