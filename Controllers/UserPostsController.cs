using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XploreIN;
using XploreIN.Models;

namespace XploreIN.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserPostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public UserPostsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/UserPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPost>>> GetUserPosts()
        {
            // Get the authenticated user's email from the token
            string email = User.FindFirst(ClaimTypes.Email)?.Value;

            var userPosts = await _context.UserPosts.Where(u => u.User.Email == email).ToListAsync();
            if (userPosts == null)
          {
              return NotFound();
          }
            return userPosts;
        }

        // GET: api/UserPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPost>> GetUserPost(int id)
        {
          if (_context.UserPosts == null)
          {
              return NotFound();
          }
            var userPost = await _context.UserPosts.FindAsync(id);

            if (userPost == null)
            {
                return NotFound();
            }

            return userPost;
        }

        // PUT: api/UserPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPost(int id, UserPost userPost)
        {
            if (id != userPost.Id)
            {
                return BadRequest();
            }

            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            IdentityUser user = await userManager.FindByEmailAsync(email);
            userPost.UserId = user.Id;
            userPost.User = user;
            _context.Entry(userPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserPosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserPost>> PostUserPost(UserPost userPost)
        {
          if (_context.UserPosts == null)
          {
              return Problem("Entity set 'ApplicationDbContext.UserPosts'  is null.");
          }
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            IdentityUser user = await userManager.FindByEmailAsync(email);
            userPost.UserId = user.Id;
            userPost.User = user;
            _context.UserPosts.Add(userPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserPost", new { id = userPost.Id }, userPost);
        }

        // DELETE: api/UserPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPost(int id)
        {
            if (_context.UserPosts == null)
            {
                return NotFound();
            }
            var userPost = await _context.UserPosts.FindAsync(id);
            if (userPost == null)
            {
                return NotFound();
            }

            _context.UserPosts.Remove(userPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserPostExists(int id)
        {
            return (_context.UserPosts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
