using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using XploreIN.Controllers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace XploreIN.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly byte[] _key;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _tokenHandler = new JwtSecurityTokenHandler();
            _key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey is null"));
        }
        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email ,user.Email)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = _tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = _tokenHandler.WriteToken(token);

            return tokenString;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModelNew model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var tokenString = GenerateJwtToken(user);

                return Ok(new { Token = tokenString, UserName = user.UserName });

            }

            return BadRequest("Invalid username or password.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] IdentityUser user)
        {
            string Email = user.Email;
            string Password = user.PasswordHash;
            var tempUser = CreateUser();
            await _userStore.SetUserNameAsync(tempUser, Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(tempUser, Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(tempUser, Password);

            if (result.Succeeded)
            {
                tempUser.EmailConfirmed = true;
                await _userManager.UpdateAsync(tempUser);
                // Add user to a role
                await _userManager.AddToRoleAsync(tempUser, "User");
                //  var tokenString = GenerateJwtToken(user);
                //   return Ok(new { Token = tokenString });
                return Ok();
            }
            else
            {
                //// Handle errors and return appropriate response
                //return BadRequest(result.Errors);


                // Extract the error descriptions
                var errorDescriptions = result.Errors.Select(error => error.Description);

                // Return the error descriptions in the response
                return BadRequest(new { Errors = errorDescriptions });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Sign out the user
            return Ok();
        }

        public class LoginModelNew
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }
    }
}


