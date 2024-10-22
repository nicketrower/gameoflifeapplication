namespace GameOfLifeAPI.Models
{
    public class GameBoardState
    {
        public required SessionState SessionState { get; set; }
        public required List<List<int>> GameBoardArr { get; set; }
    }
}
