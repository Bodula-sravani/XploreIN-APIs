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
    public class DestinationMediasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DestinationMediasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DestinationMedias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DestinationMedia>>> GetdestinationMedias()
        {
          if (_context.destinationMedias == null)
          {
              return NotFound();
          }
            return await _context.destinationMedias.ToListAsync();
        }

        // GET: api/DestinationMedias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DestinationMedia>>> GetDestinationMedia(int id)
        {
          if (_context.destinationMedias == null)
          {
              return NotFound();
          }
            var destinationMedia = await _context.destinationMedias.Where(dm => dm.Destination_id == id).ToListAsync();

            if (destinationMedia == null)
            {
                return NotFound();
            }

            return destinationMedia;
        }

        // PUT: api/DestinationMedias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestinationMedia(int id, DestinationMedia destinationMedia)
        {
            if (id != destinationMedia.Id)
            {
                return BadRequest();
            }

            _context.Entry(destinationMedia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationMediaExists(id))
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

        // POST: api/DestinationMedias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DestinationMedia>> PostDestinationMedia(DestinationMedia destinationMedia)
        {
          if (_context.destinationMedias == null)
          {
              return Problem("Entity set 'ApplicationDbContext.destinationMedias'  is null.");
          }
            int destination_id = destinationMedia.Destination_id;
            var ruralDestination = await _context.ruralDestinations.FindAsync(destination_id);
            destinationMedia.RuralDestination = ruralDestination;
            _context.destinationMedias.Add(destinationMedia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDestinationMedia", new { id = destinationMedia.Id }, destinationMedia);
        }

        // DELETE: api/DestinationMedias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestinationMedia(int id)
        {
            if (_context.destinationMedias == null)
            {
                return NotFound();
            }
            var destinationMedia = await _context.destinationMedias.FindAsync(id);
            if (destinationMedia == null)
            {
                return NotFound();
            }

            _context.destinationMedias.Remove(destinationMedia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DestinationMediaExists(int id)
        {
            return (_context.destinationMedias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
