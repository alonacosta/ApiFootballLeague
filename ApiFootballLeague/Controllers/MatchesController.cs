using ApiFootballLeague.Models;
using ApiFootballLeague.Repositories;
using ApiFootballLeague.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchRepository _matchRepository;

        public MatchesController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRounds()
        {
            IEnumerable<Round> _rounds;
            IEnumerable<RoundViewModel> _roundsViewModel;

            try
            {
                _rounds = await _matchRepository.GetRoundsAsync();

                if (_rounds == null || !_rounds.Any())
                {
                    return NotFound($"Not found rounds.");
                }

                _roundsViewModel = _rounds.Select(r => new RoundViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    DateStart = r.DateStart,
                    DateEnd = r.DateEnd,
                    IsClosed = r.IsClosed,
                });

                return Ok(_roundsViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error no round list: {ex.Message}");
            }
        }

        [HttpGet("[action]/{roundId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMatchesByRound(int roundId)
        {
            if (roundId <= 0)
            {
                return BadRequest("Round ID is required");
            }

            IEnumerable<Match> _matches;
            //IEnumerable<MatchViewModel> _matchesViewModel;
            var matchViewModels = new List<MatchViewModel>();

            try
            {
                _matches = await _matchRepository.GetMatchesByRoundIdAsync(roundId);

                if (_matches == null || !_matches.Any())
                {
                    return NotFound($"Matches not found.");
                }

                //var matchViewModels = new List<MatchViewModel>();
                foreach (var m in _matches)
                {
                    var homeTeamImageUrl = await _matchRepository.GetImageClubUrl(m.HomeTeam);
                    var awayTeamImageUrl = await _matchRepository.GetImageClubUrl(m.AwayTeam);

                    var matchViewModel = new MatchViewModel
                    {
                        Id = m.Id,
                        HomeTeam = m.HomeTeam,
                        AwayTeam = m.AwayTeam,
                        HomeScore = m.HomeScore,
                        AwayScore = m.AwayScore,
                        IsClosed = m.IsClosed,
                        StartDate = m.StartDate,
                        IsFinished = m.IsFinished,
                        ImageHomeTeamUrl = homeTeamImageUrl,
                        ImageAwayTeamUrl = awayTeamImageUrl
                    };

                    matchViewModels.Add(matchViewModel);
                }
                //_matchesViewModel = await Task.WhenAll( _matches.Select(async m => new MatchViewModel
                //{                    
                //    Id = m.Id,
                //    HomeTeam = m.HomeTeam,
                //    AwayTeam = m.AwayTeam,
                //    HomeScore = m.HomeScore,
                //    AwayScore = m.AwayScore,
                //    IsClosed = m.IsClosed,
                //    StartDate = m.StartDate,
                //    IsFinished = m.IsFinished,
                //    ImageHomeTeamUrl = await _matchRepository.GetImageClubUrl(m.HomeTeam),
                //    ImageAwayTeamUrl = await _matchRepository.GetImageClubUrl(m.AwayTeam)
                //}));

                return Ok(matchViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error no Match: {ex.Message}");
            }
        }
    }
}
