using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XploreIN;
using XploreIN.Models;

namespace XploreIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuralDestinationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RuralDestinationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RuralDestinations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuralDestination>>> GetruralDestinations()
        {
          if (_context.ruralDestinations == null)
          {
              return NotFound();
          }
            return await _context.ruralDestinations.ToListAsync();
        }
        // GET: api/RuralDestinations/5
    


        [HttpGet("{name}")]
        public async Task<ActionResult<RuralDestination>> GetRuralDestination(string name)
        {
            if (_context.ruralDestinations == null)
            {
                return NotFound();
            }

            var ruralDestination = await _context.ruralDestinations.Where(dm => EF.Functions.Like(dm.Name.ToLower(), $"%{name.ToLower()}%")).FirstOrDefaultAsync();
         


                //Where(dm => EF.Functions.Like(dm.Name.ToLower(), $"%{name.ToLower()}%"));

            if (ruralDestination == null)
            {
                return NotFound();
            }

            return ruralDestination;
        }

        // PUT: api/RuralDestinations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRuralDestination(int id, RuralDestination ruralDestination)
        {
            if (id != ruralDestination.Id)
            {
                return BadRequest();
            }

            _context.Entry(ruralDestination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RuralDestinationExists(id))
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

        // POST: api/RuralDestinations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RuralDestination>> PostRuralDestination(RuralDestination ruralDestination)
        {
          if (_context.ruralDestinations == null)
          {
              return Problem("Entity set 'ApplicationDbContext.ruralDestinations'  is null.");
          }
            _context.ruralDestinations.Add(ruralDestination);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRuralDestination", new { id = ruralDestination.Id }, ruralDestination);
        }

        // DELETE: api/RuralDestinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRuralDestination(int id)
        {
            if (_context.ruralDestinations == null)
            {
                return NotFound();
            }
            var ruralDestination = await _context.ruralDestinations.FindAsync(id);
            if (ruralDestination == null)
            {
                return NotFound();
            }

            _context.ruralDestinations.Remove(ruralDestination);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RuralDestinationExists(int id)
        {
            return (_context.ruralDestinations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
