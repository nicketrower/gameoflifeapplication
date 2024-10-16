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

        [HttpGet("GetNextState")]
        public async Task<string> GetNextStateAsync()
        {
           return await _gameOfLife.GetNextStateAsync();
         
        }

        [HttpGet("GetFutureState")]
        public async Task<MockDTO> GetFurtureStateAsync()
        {
           return  await _gameOfLife.GetFurtureStateAsync();
           
        }

        [HttpGet("GetFinalState")]
        public string GetFinalState()
        {
            _gameOfLife.GetFinalStateAsync();
            return "Get Future State";
        }

        [HttpPost("PostState")]
        public string PostStateAsync()
        {
            _gameOfLife.PostState();
            return "Get Future State";
        }
    }
}
