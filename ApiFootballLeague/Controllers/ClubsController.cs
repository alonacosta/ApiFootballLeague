using ApiFootballLeague.Helpers;
using ApiFootballLeague.Models;
using ApiFootballLeague.Repositories;
using ApiFootballLeague.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiFootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly IClubRepository _clubRepository;
        private readonly IBlobHelper _blobHelper;

        public ClubsController(IClubRepository clubRepository, IBlobHelper blobHelper)
        {
            _clubRepository = clubRepository;
            _blobHelper = blobHelper;
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

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayersPositions()
        {
            IEnumerable<Models.Position> _positions;
            IEnumerable<PositionViewModel> _positionsViewModel;

            try
            {
                _positions = await _clubRepository.GetPositionsAsync();

                if (_positions == null || !_positions.Any())
                {
                    return NotFound($"Not found positions.");
                }

                _positionsViewModel = _positions.Select(c => new PositionViewModel
                {
                    Id = c.Id,
                    Name = c.Name,

                });

                return Ok(_positionsViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error no position list: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePlayer([FromBody] PlayerInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var playerIds = await _clubRepository.GetPlayersIds();
            var lastId = playerIds[playerIds.Count - 1];

            var player = new Player
            {
                Id = lastId + 1,
                Name = model.Name,
                ClubId = model.ClubId,
                PositionId = model.PositionId,
                ImageId = Guid.Empty,
            };

            var result = await _clubRepository.CreatePlayerAsync(player);

            if (!result)
            {
                return BadRequest("The player was not created.");
            }

            //return Ok("Email confirmed successfully!");
            return StatusCode(StatusCodes.Status201Created, $"Player created successfully!");
        }


        //[Authorize]
        [HttpPost("UploadPhotoPlayer")]
        public async Task<IActionResult> UploadPhotoPlayer(IFormFile file, [FromForm] int playerId)
        {
            var player = await _clubRepository.GetPlayerByIdAsync(playerId);

            if (player == null)
            {
                return NotFound("Player not found");
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest("No images uploaded");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Invalid file type. Only images are allowed.");
            }

            Guid imageId = await _blobHelper.UploadBlobAsync(file, "players");

            player.ImageId = imageId;

            await _clubRepository.UpdatePlayerAsync(player);

            return Ok("Image uploaded successfully");
        }

        [HttpPut("UpdatePlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePlayer([FromQuery] int playerId, [FromBody] PlayerInViewModel player)
        {
            var playerOld = await _clubRepository.GetPlayerByIdAsync(playerId);

            if (player == null)
            {
                return NotFound("Player not found");
            }

            playerOld.Id = playerId;
            playerOld.Name = player.Name;
            playerOld.ClubId = player.ClubId;
            playerOld.PositionId = player.PositionId;            

            await _clubRepository.UpdatePlayerAsync(playerOld);

            return Ok("Player updated successfully");
        }


        [HttpDelete("DeletePlayer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlayer([FromQuery] int playerId)
        {            
            var player = await _clubRepository.GetPlayerByIdAsync(playerId);

            if (player == null)
            {
                return NotFound("Player not found");
            }

            await _clubRepository.DeletePlayerAsync(player);

            return Ok("Player deleted successfully");
        }
    }
}
