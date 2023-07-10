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
    public class UserItinerariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public UserItinerariesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/UserItineraries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItineraries>>> GetUserItineraries()
        {
            // Get the authenticated user's email from the token
            string email = User.FindFirst(ClaimTypes.Email)?.Value;

            // Retrieve the user's itineraries from the database
            var userItineraries = await _context.UserItineraries
                .Where(u => u.User.Email == email)
                .ToListAsync();

            if (userItineraries == null)
            {
                return NotFound();
            }

            return userItineraries;
        }

        // GET: api/UserItineraries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserItineraries>> GetUserItineraries(int id)
        {
          if (_context.UserItineraries == null)
          {
              return NotFound();
          }
            var userItineraries = await _context.UserItineraries
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (userItineraries == null)
            {
                return NotFound();
            }

            return userItineraries;
        }

        // PUT: api/UserItineraries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItineraries(int id, UserItineraries userItineraries)
        {
            if (id != userItineraries.Id)
            {
                return BadRequest();
            }
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            IdentityUser user = userManager.FindByEmailAsync(email).Result;
            userItineraries.UserId = user.Id;
            userItineraries.User = user;

            _context.Entry(userItineraries).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserItinerariesExists(id))
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

        // POST: api/UserItineraries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserItineraries>> PostUserItineraries(UserItineraries userItineraries)
        {
          if (_context.UserItineraries == null)
          {
              return Problem("Entity set 'ApplicationDbContext.UserItineraries'  is null.");
          }
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            IdentityUser user = userManager.FindByEmailAsync(email).Result;
            userItineraries.UserId = user.Id;
            userItineraries.User = user;
            _context.UserItineraries.Add(userItineraries);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserItineraries", new { id = userItineraries.Id }, userItineraries);
        }

        // DELETE: api/UserItineraries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserItineraries(int id)
        {
            if (_context.UserItineraries == null)
            {
                return NotFound();
            }
            var userItineraries = await _context.UserItineraries.FindAsync(id);
            if (userItineraries == null)
            {
                return NotFound();
            }

            _context.UserItineraries.Remove(userItineraries);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool UserItinerariesExists(int id)
        {
            return (_context.UserItineraries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
