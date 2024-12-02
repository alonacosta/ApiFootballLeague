using ApiFootballLeague.Models;
using ApiFootballLeague.ViewModels;

namespace ApiFootballLeague.Repositories
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Round>> GetRoundsAsync();
        Task<IEnumerable<Match>> GetMatchesByRoundIdAsync(int roundId);
        Task<List<StatisticsViewModel>> CalculateStatisticsAsync();
    }
}
