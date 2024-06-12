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
    public class InksController : ControllerBase
    {
        private readonly DataContext _context;

        public InksController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Inks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ink>>> GetInks()
        {
            return await _context.Inks.ToListAsync();
        }

        // GET: api/Inks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ink>> GetInk(int id)
        {
            var ink = await _context.Inks.FindAsync(id);

            if (ink == null)
            {
                return NotFound();
            }

            return ink;
        }

        // PUT: api/Inks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInk(int id, Ink ink)
        {
            if (id != ink.Id)
            {
                return BadRequest();
            }

            _context.Entry(ink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InkExists(id))
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

        // POST: api/Inks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ink>> PostInk(Ink ink)
        {
            _context.Inks.Add(ink);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInk", new { id = ink.Id }, ink);
        }

        // DELETE: api/Inks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInk(int id)
        {
            var ink = await _context.Inks.FindAsync(id);
            if (ink == null)
            {
                return NotFound();
            }

            _context.Inks.Remove(ink);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InkExists(int id)
        {
            return _context.Inks.Any(e => e.Id == id);
        }
    }
}
