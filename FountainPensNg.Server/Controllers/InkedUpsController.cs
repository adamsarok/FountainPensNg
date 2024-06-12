using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Models;

namespace FountainPensNg.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InkedUpsController : ControllerBase
    {
        private readonly DataContext _context;

        public InkedUpsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/InkedUps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InkedUp>>> GetInkedUps()
        {
            return await _context.InkedUps.ToListAsync();
        }

        // GET: api/InkedUps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InkedUp>> GetInkedUp(int id)
        {
            var inkedUp = await _context.InkedUps.FindAsync(id);

            if (inkedUp == null)
            {
                return NotFound();
            }

            return inkedUp;
        }

        // PUT: api/InkedUps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInkedUp(int id, InkedUp inkedUp)
        {
            if (id != inkedUp.Id)
            {
                return BadRequest();
            }

            _context.Entry(inkedUp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InkedUpExists(id))
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

        // POST: api/InkedUps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InkedUp>> PostInkedUp(InkedUp inkedUp)
        {
            _context.InkedUps.Add(inkedUp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInkedUp", new { id = inkedUp.Id }, inkedUp);
        }

        // DELETE: api/InkedUps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInkedUp(int id)
        {
            var inkedUp = await _context.InkedUps.FindAsync(id);
            if (inkedUp == null)
            {
                return NotFound();
            }

            _context.InkedUps.Remove(inkedUp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InkedUpExists(int id)
        {
            return _context.InkedUps.Any(e => e.Id == id);
        }
    }
}
