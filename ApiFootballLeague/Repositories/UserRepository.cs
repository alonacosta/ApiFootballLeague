using ApiFootballLeague.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiFootballLeague.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LeagueDbContext _context;

        public UserRepository(LeagueDbContext context)
        {
            _context = context;
        }
        public async Task<AspNetUser> GetUserByEmailAsync(string email)
        {
            return await _context.AspNetUsers
                .Where(anu => anu.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateUserAsync(AspNetUser user)
        {
            _context.AspNetUsers.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<AspNetUser> GetUserByIdAsync(string userId)
        {
            return await _context.AspNetUsers
                .Where(anu => anu.Id == userId)
                .FirstOrDefaultAsync();
        }
    }
}
