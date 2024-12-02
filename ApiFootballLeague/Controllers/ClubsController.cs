using ApiFootballLeague.Models;
using ApiFootballLeague.Repositories;
using ApiFootballLeague.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubRepository _clubRepository;

        public ClubsController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClubs()
        {
            IEnumerable<Club> _clubs;
            IEnumerable<ClubViewModel> _clubsViewModel;

            try
            {
                _clubs = await _clubRepository.GetClubsAsync();

                if (_clubs == null || !_clubs.Any())
                {
                    return NotFound($"Not found clubs.");
                }

                _clubsViewModel = _clubs.Select(c => new ClubViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Stadium = c.Stadium,
                    Capacity = c.Capacity,
                    HeadCoach = c.HeadCoach,
                    ImageFullPath = c.ImageFullPath,
                });

                return Ok(_clubsViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error no club list: {ex.Message}");
            }
        }

        [HttpGet("[action]/{clubId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayersByClub(int clubId)
        {
            if (clubId <= 0)
            {
                return BadRequest("Club ID is required");
            }

            IEnumerable<Player> _players;
            IEnumerable<PlayerViewModel> _playersViewModel;

            try
            {
                _players = await _clubRepository.GetTeamByClubIdAsync(clubId);

                if (_players == null || !_players.Any())
                {
                    return NotFound($"Players not found.");
                }

                _playersViewModel = _players.Select(p => new PlayerViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ClubName = p.Club.Name,
                    PositionName = p.Position.Name,
                    ImageUrl = p.ImageUrl,
                });

                return Ok(_playersViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error no Team: {ex.Message}");
            }
        }
    }
}
