using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Models;
using System.Text.Json;
using AutoMapper;
using FountainPensNg.Server.Data.DTO;

namespace FountainPensNg.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FountainPensController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FountainPensController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/FountainPens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FountainPenDTO>>> GetFountainPens()
        {
            var temp = await _context
                .FountainPens
                .Include(x => x.CurrentInk)
                .Include(x => x.InkedUps)
                .ToListAsync();
            var res = new List<FountainPenDTO>();
            foreach (var f in temp) {
                res.Add(MapFountainPen(f));
            }
            return res;
        }

        private FountainPenDTO MapFountainPen(FountainPen f) {
            var dto = _mapper.Map<FountainPenDTO>(f);
            dto.CurrentInk = _mapper.Map<InkForListDTO>(f.CurrentInk);
            dto.InkedUps = _mapper.Map<List<InkedUpForListDTO>>(f.InkedUps);
            return dto;
        }

        // GET: api/FountainPens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FountainPenDTO>> GetFountainPen(int id)
        {
            var fountainPen = await _context.FountainPens
                .Include(pen => pen.CurrentInk)
                .Include(pen => pen.InkedUps)
                .ThenInclude(inkup => inkup.Ink)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (fountainPen == null)
            {
                return NotFound();
            }

            return MapFountainPen(fountainPen);
        }

        // PUT: api/FountainPens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFountainPen(int id, FountainPenDTO dto)
        {
            var fountainPen = _mapper.Map<FountainPen>(dto);
            if (fountainPen == null || id != fountainPen.Id)
            {
                return BadRequest();
            }

            
            var old = await _context.FountainPens.FindAsync(fountainPen.Id);
            if (old != null) {
                if (old != null 
                    && old.CurrentInkId != fountainPen.CurrentInkId 
                    && fountainPen.CurrentInkId.HasValue
                    && fountainPen.CurrentInkId != 0) {
                    _context.InkedUps.Add(new InkedUp() {
                        FountainPenId = fountainPen.Id,
                        InkId = fountainPen.CurrentInkId.Value,
                        MatchRating = fountainPen.CurrentInkRating ?? 0
                    });
                }
                
                _context.Entry(old).CurrentValues.SetValues(fountainPen);
                old.ModifiedAt = DateTime.UtcNow;
                //_context.Entry(fountainPen).State = EntityState.Modified;
            }

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
        public async Task<ActionResult<FountainPen>> PostFountainPen(FountainPenDTO dto)
        {
            var fountainPen = _mapper.Map<FountainPen>(dto);
            if (fountainPen == null)
            {
                return BadRequest();
            }
            _context.FountainPens.Add(fountainPen);
            if (fountainPen.CurrentInkId.HasValue && fountainPen.CurrentInkId > 0) {
                _context.InkedUps.Add(new InkedUp() {
                    FountainPen = fountainPen,
                    InkId = fountainPen.CurrentInkId.Value,
                    MatchRating = fountainPen.CurrentInkRating ?? 0
                });
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFountainPen", new { id = fountainPen.Id }, dto);
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
