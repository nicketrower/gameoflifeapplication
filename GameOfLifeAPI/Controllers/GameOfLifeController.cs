using GameOfLifeAPI.Interfaces;
using GameOfLifeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameOfLifeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameOfLifeController : ControllerBase
    {
       

        private readonly ILogger<GameOfLifeController> _logger;
        private readonly IGameOfLifeService _gameOfLife;

        public GameOfLifeController(ILogger<GameOfLifeController> logger, IGameOfLifeService gameOfLife)
        {
            _logger = logger;
            _gameOfLife = gameOfLife;
        }

        [HttpPost("GetNextState")]
        public async Task<IEnumerable<Cell>> GetNextStateAsync([FromBody] IEnumerable<Cell> seed)
        {
           return await _gameOfLife.GetNextStateAsync(seed);
         
        }

        [HttpGet("GetFutureState")]
        public async Task<MockDTO> GetFurtureStateAsync()
        {
           return  await _gameOfLife.GetFurtureStateAsync();
           
        }

        [HttpGet("GetFinalState")]
        public async Task<string> GetFinalStateAsync()
        {
            await _gameOfLife.GetFinalStateAsync();
            return "Get Future State";
        }

        [HttpPost("PostState")]
        public async Task<MockDTO> PostStateAsync([FromBody] MockDTO mockDTO)
        {
          return await _gameOfLife.PostStateAsync(mockDTO);
        
        }
    }
}
