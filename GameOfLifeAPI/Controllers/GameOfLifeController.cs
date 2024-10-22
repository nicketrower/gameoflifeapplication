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
        /// POST Create a new board based on seed data
        /// </summary>
        /// <param name="sessionState">The session state containing the board details.</param>
        /// <returns>
        /// code = 200, message = "OK", response = GameBoardState
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpPost("CreateNewBoard")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNewBoardAsync([FromBody] SessionState sessionState)
        {
            try
            {
                var result = await _gameOfLife.CreateNewBoard(sessionState);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid arguments provided for CreateNewBoardAsync.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the new board.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// GET All Stored Boards
        /// </summary>
        /// <returns>
        /// code = 200, message = "OK", response = Cell
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpGet("GetStoredBoards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStoredBoards()
        {
            try
            {
                return Ok(await _gameOfLife.GetStoredBoards());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        

        /// <summary>
        /// GET Next State based on Seed Data
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <returns>
        /// code = 200, message = "OK", response = Cell
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpGet("GetNextState/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNextStateAsync(string id)
        {
            try
            {
                return Ok(await _gameOfLife.GetNextStateAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// GET Future State based on Id and Future Count
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <param name="futurecount">The number of future states to calculate.</param>
        /// <returns>
        /// code = 200, message = "OK", response = List of List of integers representing the future state of the board
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpGet("GetFutureState/{id}/{futurecount}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFutureStateAsync(string id, int futurecount)
        {
            try
            {
                var futureState = await _gameOfLife.GetFurtureStateAsync(id, futurecount);
                return Ok(futureState);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid arguments provided for GetFutureStateAsync.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the future state.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// GET Final State based on Id
        /// </summary>
        /// <param name="id">The unique identifier of the board.</param>
        /// <returns>
        /// code = 200, message = "OK", response = List of List of integers representing the final state of the board
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpGet("GetFinalStateAsync/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFinalStateAsync(string id)
        {
            try
            {
                var finalState = await _gameOfLife.GetFinalStateAsync(id);
                return Ok(finalState);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid arguments provided for GetFinalStateAsync.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the final state.");
                return StatusCode(StatusCodes.Status400BadRequest, "An error occurred while getting the final state.");
            }
        }


        /// <summary>
        /// POST New Board State or Update Existing Board State
        /// </summary>
        /// <returns>
        /// code = 200, message = "OK", response = IActionResult
        /// code = 400, message = "BAD_REQUEST", response = Exception
        /// code = 500, message = "INTERNAL_SERVER_ERROR", response = Exception
        /// </returns>
        [HttpPut("UpdateBoardState")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBoardState([FromBody] GameBoardState gameBoardState)
        {
            try
            {
                var result = await _gameOfLife.UpdateBoardState(gameBoardState);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid arguments provided for {MethodName}.", nameof(UpdateBoardState));
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the board state.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
