using ApiFootballLeague.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFootballLeague.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly LeagueDbContext _context;

        public ClubRepository(LeagueDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Club>> GetClubsAsync()
        {
            return await _context.Clubs             
             .OrderBy(g => g.Name)
             .ToListAsync();
        }

        public async Task<IEnumerable<Player>> GetTeamByClubIdAsync(int clubId)
        {
            return await _context.Players
                .Include(p => p.Club)
                .Include(p => p.Position)
                .Where(p => p.ClubId == clubId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetPositionsAsync()
        {
            return await _context.Positions                
                .ToListAsync();
        }
       
        public async Task<bool> CreatePlayerAsync(Player player)
        {
            if (player == null) { return false; }
            await _context.Players.AddAsync(player);
            int affectedRows = await _context.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<List<int>> GetPlayersIds()
        {
            return await _context.Players
                .Select(p => p.Id).ToListAsync();
        }

        public async Task<Player> GetPlayerByIdAsync(int playerId)
        {
            return await _context.Players
                .Where(p => p.Id == playerId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlayerAsync(Player player)
        {
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
        }

    }
}
