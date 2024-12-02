using ApiFootballLeague.ViewModels;

namespace ApiFootballLeague.Repositories
{
    public interface IMatchRepository
    {
        Task<List<StatisticsViewModel>> CalculateStatisticsAsync();
    }
}
