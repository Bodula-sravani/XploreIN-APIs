using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace XploreIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;

        public UserController(UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] IdentityUser user)
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
            //    var result = await _userManager.CreateAsync(user);
            //if (result.Succeeded)
            //{
                // User created successfully
                return Ok();
            }
            else
            {
                // Handle errors and return appropriate response
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Return the user
                return Ok(user);
            }
            else
            {
                // User not found
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] IdentityUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            if (existingUser != null)
            {
                // Update the user properties
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    // User updated successfully
                    return Ok();
                }
                else
                {
                    // Handle errors and return appropriate response
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                // User not found
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    // User deleted successfully
                    return Ok();
                }
                else
                {
                    // Handle errors and return appropriate response
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                // User not found
                return NotFound();
            }
        }
    }
}
