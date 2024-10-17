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
        public GameOfLifeController(ILogger<GameOfLifeController> logger, IGameOfLifeService gameOfLife)
        {
            _logger = logger;
            _gameOfLife = gameOfLife;
        }

        /// <summary>
        /// GET Next State based on Seed Data
        /// </summary>
        /// <returns>
        /// code = 200, message = "OK", response = Cell
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpPost("GetNextState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNextStateAsync([FromBody] IEnumerable<Cell> seed)
        {
            try
            {
                return Ok(await _gameOfLife.GetNextStateAsync(seed));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// GET Future State based on Seed Data
        /// </summary>
        /// <returns>
        /// code = 200, message = "OK", response = Cell
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpGet("GetFutureState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<MockDTO> GetFurtureStateAsync()
        {
           return  await _gameOfLife.GetFurtureStateAsync();
           
        }

        /// <summary>
        /// GET Future State based on Seed Data
        /// </summary>
        /// <returns>
        /// code = 200, message = "OK", response = Cell
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpGet("GetFinalState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<string> GetFinalStateAsync()
        {
            await _gameOfLife.GetFinalStateAsync();
            return "Get Future State";
        }

        /// <summary>
        /// GET Future State based on Seed Data
        /// </summary>
        /// <returns>
        /// code = 200, message = "OK", response = Cell
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpPost("PostState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<MockDTO> PostStateAsync([FromBody] MockDTO mockDTO)
        {
          return await _gameOfLife.PostStateAsync(mockDTO);
        
        }
    }
}
