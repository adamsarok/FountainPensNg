using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace FountainPensNg.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InkedUpsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public InkedUpsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/InkedUps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InkedUpDTO>>> GetInkedUps()
        {
            var query =_context.InkedUps
                .Include(x => x.Ink)
                .Include(x => x.FountainPen)
                .AsQueryable();
            return await query
                .ProjectTo<InkedUpDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/InkedUps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InkedUpDTO>> GetInkedUp(int id)
        {
            var query =_context.InkedUps
                .Include(x => x.Ink)
                .Include(x => x.FountainPen)
                .Where(x => x.Id == id)
                .AsQueryable();
            var inkedUp = await query
                .ProjectTo<InkedUpDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (inkedUp == null)
            {
                return NotFound();
            }

            return inkedUp;
        }

        // PUT: api/InkedUps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInkedUp(int id, InkedUpDTO dto)
        {
            var inkedUp = _mapper.Map<InkedUp>(dto);
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
        [HttpPost]
        public async Task<ActionResult<InkedUp>> PostInkedUp(InkedUpDTO dto)
        {
            var inkedUp = _mapper.Map<InkedUp>(dto); //something really cursed is happening here w automapper
            inkedUp.Ink = null;
            inkedUp.FountainPen = null;
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
