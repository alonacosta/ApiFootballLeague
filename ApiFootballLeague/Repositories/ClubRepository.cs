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
    }
}
