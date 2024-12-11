using ApiFootballLeague.Models;

namespace ApiFootballLeague.Repositories
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetClubsAsync();
        Task<IEnumerable<Player>> GetTeamByClubIdAsync(int clubId);
        Task<IEnumerable<Position>> GetPositionsAsync();
        Task<bool> CreatePlayerAsync(Player player);
        Task<List<int>> GetPlayersIds();
        Task<Player> GetPlayerByIdAsync(int playerId);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(Player player);
    }
}
