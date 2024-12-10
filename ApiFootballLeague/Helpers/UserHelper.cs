using ApiFootballLeague.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiFootballLeague.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly LeagueDbContext _context;

        public UserHelper(UserManager<AspNetUser> userManager, LeagueDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

        public async Task<string> GetUserRoleAsync(AspNetUser user)
        {
            //return (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            //var roles = await _context.AspNetUserRoles
            //    .Where(ur => ur.UserId == user.Id)
            //    .Select(ur => ur.RoleId)
            //    .ToListAsync();
            var userRole = await _context.AspNetUserRoles
            .Include(ur => ur.Role)         
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.Role.Name) 
            .FirstOrDefaultAsync();

            return userRole ?? "Role not found";



        }

        public async Task<string> GetUserRoleFromClaimsAsync(AspNetUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim?.Value;
        }




    }
}
