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
    public class ItineraryItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public ItineraryItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/ItineraryItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItineraryItem>>> GetItineraryItems()
        {
          if (_context.ItineraryItems == null)
          {
              return NotFound();
          }
            return await _context.ItineraryItems.ToListAsync();
        }

        // GET: api/ItineraryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ItineraryItem>>> GetItineraryItem(int id)
        {
          if (_context.ItineraryItems == null)
          {
              return NotFound();
          }
            var itineraryItem = await _context.ItineraryItems.Where(i => i.ItineraryId == id).ToListAsync();

            if (itineraryItem == null)
            {
                return NotFound();
            }

            return itineraryItem;
        }

        // PUT: api/ItineraryItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItineraryItem(int id, ItineraryItem itineraryItem)
        {
            if (id != itineraryItem.Id)
            {
                return BadRequest();
            }
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            IdentityUser user = userManager.FindByEmailAsync(email).Result;
            UserItineraries temp = await _context.UserItineraries.FirstOrDefaultAsync(u => u.Id == itineraryItem.ItineraryId);
            itineraryItem.ItineraryId = temp.Id;
            itineraryItem.UserItineraries = temp;
            itineraryItem.UserItineraries.UserId = user.Id;
            itineraryItem.UserItineraries.User = user;
            _context.Entry(itineraryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItineraryItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/ItineraryItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItineraryItem>> PostItineraryItem(ItineraryItem itineraryItem)
        {
          if (_context.ItineraryItems == null)
          {
              return Problem("Entity set 'ApplicationDbContext.ItineraryItems'  is null.");
          }
            string email = User.FindFirst(ClaimTypes.Email)?.Value;
            IdentityUser user = userManager.FindByEmailAsync(email).Result;
            UserItineraries temp = await _context.UserItineraries.FirstOrDefaultAsync(u => u.Id == itineraryItem.ItineraryId);
            itineraryItem.ItineraryId = temp.Id;
            itineraryItem.UserItineraries = temp;
            itineraryItem.UserItineraries.UserId = user.Id;
            itineraryItem.UserItineraries.User = user;
            _context.ItineraryItems.Add(itineraryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItineraryItem", new { id = itineraryItem.Id }, itineraryItem);
        }

        // DELETE: api/ItineraryItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItineraryItem(int id)
        {
            if (_context.ItineraryItems == null)
            {
                return NotFound();
            }
            var itineraryItem = await _context.ItineraryItems.FindAsync(id);
            if (itineraryItem == null)
            {
                return NotFound();
            }

            _context.ItineraryItems.Remove(itineraryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItineraryItemExists(int id)
        {
            return (_context.ItineraryItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
