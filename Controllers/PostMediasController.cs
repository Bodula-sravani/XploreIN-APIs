using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XploreIN;
using XploreIN.Migrations;
using XploreIN.Models;

namespace XploreIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostMediasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostMediasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PostMedias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostMedia>>> GetpostMedias()
        {
          if (_context.postMedias == null)
          {
              return NotFound();
          }
            return await _context.postMedias.ToListAsync();
        }

        // GET: api/PostMedias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<PostMedia>>> GetPostMedia(int id)
        {
          if (_context.postMedias == null)
          {
              return NotFound();
          }
            var postMedia = await _context.postMedias.Where(pm=> pm.PostId== id).ToListAsync();

            if (postMedia == null)
            {
                return NotFound();
            }

            return postMedia;
        }

        // PUT: api/PostMedias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostMedia(int id, PostMedia postMedia)
        {
            if (id != postMedia.Id)
            {
                return BadRequest();
            }

            _context.Entry(postMedia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostMediaExists(id))
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

        // POST: api/PostMedias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostMedia>> PostPostMedia(PostMedia postMedia)
        {
          if (_context.postMedias == null)
          {
              return Problem("Entity set 'ApplicationDbContext.postMedias'  is null.");
          }
            int post_id = postMedia.PostId;
            var userPost = await _context.UserPosts.FindAsync(post_id);
            postMedia.UserPost = userPost;
            _context.postMedias.Add(postMedia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPostMedia", new { id = postMedia.Id }, postMedia);
        }

        // DELETE: api/PostMedias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostMedia(int id)
        {
            if (_context.postMedias == null)
            {
                return NotFound();
            }
            var postMedia = await _context.postMedias.FindAsync(id);
            if (postMedia == null)
            {
                return NotFound();
            }

            _context.postMedias.Remove(postMedia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostMediaExists(int id)
        {
            return (_context.postMedias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
