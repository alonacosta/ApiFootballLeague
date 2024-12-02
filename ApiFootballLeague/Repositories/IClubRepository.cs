using ApiFootballLeague.Models;

namespace ApiFootballLeague.Repositories
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetClubsAsync();
        Task<IEnumerable<Player>> GetTeamByClubIdAsync(int clubId);
    }
}
