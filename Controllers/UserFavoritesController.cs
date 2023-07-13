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
    public class UserFavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public UserFavoritesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/UserFavorites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavorites>>> GetUserFavorites()
        {

            // Get the authenticated user's email from the token
            string email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (_context.UserFavorites == null)
          {
              return NotFound();
          }
            return await _context.UserFavorites.Where(u => u.User.Email == email).ToListAsync();
        }

        // GET: api/UserFavorites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFavorites>> GetUserFavorites(int id)
        {
          if (_context.UserFavorites == null)
          {
              return NotFound();
          }
            var userFavorites = await _context.UserFavorites.FindAsync(id);

            if (userFavorites == null)
            {
                return NotFound();
            }

            return userFavorites;
        }

        // PUT: api/UserFavorites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFavorites(int id, UserFavorites userFavorites)
        {
            if (id != userFavorites.Id)
            {
                return BadRequest();
            }

            _context.Entry(userFavorites).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFavoritesExists(id))
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

        // POST: api/UserFavorites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserFavorites>> PostUserFavorites(UserFavorites userFavorites)
        {
          if (_context.UserFavorites == null)
          {
              return Problem("Entity set 'ApplicationDbContext.UserFavorites'  is null.");
          }
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            IdentityUser user = userManager.FindByEmailAsync(email).Result;
            userFavorites.UserId = user.Id;
            userFavorites.User = user;
            _context.UserFavorites.Add(userFavorites);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetUserFavorites", new { id = userFavorites.Id }, userFavorites);
            return Ok();
        }

        // DELETE: api/UserFavorites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFavorites(int id)
        {
            if (_context.UserFavorites == null)
            {
                return NotFound();
            }
            var userFavorites = await _context.UserFavorites.FindAsync(id);
            if (userFavorites == null)
            {
                return NotFound();
            }

            _context.UserFavorites.Remove(userFavorites);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserFavoritesExists(int id)
        {
            return (_context.UserFavorites?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
