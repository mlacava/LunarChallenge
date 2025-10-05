using Common.Commands;
using Data.Models;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RocketsApi.Controllers
{
    public class RocketsController : Controller
    {
        private readonly IRocketsLogic _rocketLogic;

        public RocketsController(IRocketsLogic rocketService)
        {
            _rocketLogic = rocketService;
        }                    

        [HttpGet("rockets_list")]
        [EndpointDescription("Returns a list of all rockets, sorted by the specified field: speed, mission or type (default is 'type').")]
        public async Task<ActionResult<List<Rocket>>> GetAllRockets([FromQuery] string sortBy = "type")
        {
            try
            {
                var result = await _rocketLogic.GetAllRocketsAsync(sortBy);
                return result is null || result?.Count == 0 ? NoContent() : Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpGet("rocket")]
        [EndpointDescription("Returns a single rocket by its channel identifier.")]
        public async Task<ActionResult<Rocket>> GetRocket([FromQuery] string channel)
        {
            try
            {
                var rocket = await _rocketLogic.GetRocketAsync(channel);

                return rocket is null ? NotFound() : Ok(rocket);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("messages")]
        [EndpointDescription("Processes incoming messages to create or update rocket information.")]
        public async Task<IActionResult> Messages([FromBody] MessageCommand command)
        {
            if (command is null || command.Metadata is null || command.Message is null) return BadRequest("Invalid command");

            try
            {

                await _rocketLogic.CreateUpdateRocketAsync(command);

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
