using ApiFootballLeague.Repositories;
using ApiFootballLeague.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMatchRepository _matchRepository;

        public DashboardController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var statistics = await _matchRepository.CalculateStatisticsAsync();
                if (statistics == null || !statistics.Any())
                    return NotFound("No statistics found.");
              
                return Ok(statistics);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error in the list: {ex.Message}");
            }
        }
    }
}
