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
    public class FountainPensController : ControllerBase
    {
        private readonly DataContext _context;

        public FountainPensController(DataContext context)
        {
            _context = context;
        }

        // GET: api/FountainPens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FountainPen>>> GetFountainPens()
        {
            return await _context.FountainPens.ToListAsync();
        }

        // GET: api/FountainPens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FountainPen>> GetFountainPen(int id)
        {
            var fountainPen = await _context.FountainPens.FindAsync(id);

            if (fountainPen == null)
            {
                return NotFound();
            }

            return fountainPen;
        }

        // PUT: api/FountainPens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFountainPen(int id, FountainPen fountainPen)
        {
            if (id != fountainPen.Id)
            {
                return BadRequest();
            }

            _context.Entry(fountainPen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FountainPenExists(id))
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

        // POST: api/FountainPens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FountainPen>> PostFountainPen(FountainPen fountainPen)
        {
            _context.FountainPens.Add(fountainPen);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFountainPen", new { id = fountainPen.Id }, fountainPen);
        }

        // DELETE: api/FountainPens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFountainPen(int id)
        {
            var fountainPen = await _context.FountainPens.FindAsync(id);
            if (fountainPen == null)
            {
                return NotFound();
            }

            _context.FountainPens.Remove(fountainPen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FountainPenExists(int id)
        {
            return _context.FountainPens.Any(e => e.Id == id);
        }
    }
}
