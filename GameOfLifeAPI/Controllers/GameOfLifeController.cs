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


        /// <summary>
        /// ContactDetailController
        /// </summary>
        /// <param name="loggeer"></param>
        /// <param name="gameoflifeinterface"></param>
        public GameOfLifeController(ILogger<GameOfLifeController> logger, IGameOfLifeService gameOfLife)
        {
            _logger = logger;
            _gameOfLife = gameOfLife;
        }

        /// <summary>
        /// GET Next State based on Seed
        /// </summary>
        /// <param name="seed"></param>
        /// <returns>
        /// code = 200, message = "OK", response = EmailDTO
        /// code = 204, message = "NO_CONTENT - Record not found", response = EmailDTO
        /// code = 400, message = "BAD_REQUEST", response = EmailDTO
        /// code = 404, message = "NOT_FOUND - Error in the Request URL/Headers", response = EmailDTO
        /// code = 406, message = "NOT_ACCEPTABLE - Invalid/Missing Id", response = EmailDTO
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = EmailDTO
        /// </returns>
        [HttpPost("GetNextState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
