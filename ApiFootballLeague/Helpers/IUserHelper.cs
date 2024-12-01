using ApiFootballLeague.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiFootballLeague.Helpers
{
    public interface IUserHelper
    {
        Task<AspNetUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(AspNetUser user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(AspNetUser user);
        Task<AspNetUser> GetUserByIdAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(AspNetUser user, string token);
    }
}
