using ApiFootballLeague.Models;
using Microsoft.AspNetCore.Identity;

namespace ApiFootballLeague.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<AspNetUser> _userManager;

        public UserHelper(UserManager<AspNetUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AspNetUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> AddUserAsync(AspNetUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AspNetUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<AspNetUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<IdentityResult> ConfirmEmailAsync(AspNetUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }
        public async Task<string> GeneratePasswordResetTokenAsync(AspNetUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public async Task<IdentityResult> ResetPasswordAsync(AspNetUser user, string token, string newPassword)
        {
            if (user == null || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Invalid user, token, or password."
                });
            }

            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        
    }
}
