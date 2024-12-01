using ApiFootballLeague.Models;

namespace ApiFootballLeague.Repositories
{
    public interface IUserRepository
    {
        Task<AspNetUser> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(AspNetUser user);
        Task<AspNetUser> GetUserByIdAsync(string userId);
    }
}
