using ApiFootballLeague.Helpers;
using ApiFootballLeague.Models;
using ApiFootballLeague.Repositories;
using ApiFootballLeague.ViewModels;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiFootballLeague.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly LeagueDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public AccountController(LeagueDbContext context,
            IConfiguration configuration,
            IUserRepository userRepository,
            IBlobHelper blobHelper,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _context = context;
            _configuration = configuration;
            _userRepository = userRepository;
            _blobHelper = blobHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                return BadRequest("User already exists.");
            }          
                
            user = new AspNetUser
            {                    
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (!result.Succeeded)
            {                
                return BadRequest(result.Errors.Select(e => e.Description));
            }            

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            var response = _mailHelper.SendEmail(model.UserName, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                $"To allow the user, " +
                $"please click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Email could not be sent.");
            }
            return StatusCode(StatusCodes.Status201Created, $"User created successfully! Confirmation email sent.");   
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid confirmation link.");
            }

            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest("Email confirmation failed.");
            }

            return Ok("Email confirmed successfully!");
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginModel)
        {
            var currentUser = await _context.AspNetUsers.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (currentUser == null)
            {
                return NotFound("User don't exist");
            }

            var passwordHash = new PasswordHasher<AspNetUser>();
            var passwordVerificationResult = passwordHash.VerifyHashedPassword(currentUser, currentUser.PasswordHash, loginModel.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Incorrect password");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, currentUser.Email!)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new ObjectResult(new
            {
                accesstoken = jwt,
                tokentype = "bearer",
                userid = currentUser.Id,
                username = currentUser.UserName
            });
        }

        [Authorize]
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadUserPhoto(IFormFile file)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found");
            }

            //Guid imageId = user.ImageId;
            if (file == null || file.Length == 0)
            {
                return BadRequest("No images uploaded");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Invalid file type. Only images are allowed.");
            }

            Guid imageId = await _blobHelper.UploadBlobAsync(file, "users");

            user.ImageId = imageId;

            await _userRepository.UpdateUserAsync(user);

            return Ok("Image uploaded successfully");
        }

        [Authorize]
        [HttpGet("UserImage")]
        public async Task<IActionResult> GetUserImage()
        {
            //see if user is logged
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            //locate user
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            //var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userImage = await _context.AspNetUsers
                .Where(x => x.Email == userEmail)
                .Select(x => new
                {
                    x.ImageUrl,
                })
                .SingleOrDefaultAsync();

            return Ok(userImage);
        }

        [HttpGet("[action]/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserInfo(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("User ID is required");
            }

            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound($"User not found for the provided User ID.{userId}");
                }

                var userInfo = new UserViewModel
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    ImageUrl = user.ImageUrl,
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error no userInfo: {ex.Message}");
            }
        }
    }
}
