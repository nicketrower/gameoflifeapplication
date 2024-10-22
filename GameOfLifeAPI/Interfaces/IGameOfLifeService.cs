using GameOfLifeAPI.Models;

namespace GameOfLifeAPI.Interfaces
{
    public interface IGameOfLifeService
    {
        public Task<List<List<int>>> GetNextStateAsync(string id);
        public Task<List<List<int>>> GetFinalStateAsync(string id);
        public Task<List<List<int>>> GetFurtureStateAsync(string id,int futurecount);
        public Task<GameBoardState> CreateNewBoard(SessionState sessionState);
        public Task<GameBoardState> UpdateBoardState(GameBoardState gameBoardState);
    }
}
