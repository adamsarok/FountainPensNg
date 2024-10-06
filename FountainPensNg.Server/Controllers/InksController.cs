using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.DTO;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using FountainPensNg.Server.Helpers;

namespace FountainPensNg.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class InksController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public InksController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Inks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InkDTO>>> GetInks()
        {
            var query = _context
                .Inks
                .Include(x => x.CurrentPens != null ? x.CurrentPens.Take(1) : null)
                .AsQueryable();
            return await query
                .ProjectTo<InkDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Inks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ink>> GetInk(int id) {
            var ink = await _context.Inks.FindAsync(id);

            if (ink == null) {
                return NotFound();
            }

            return ink;
        }

        private void FillCIELab(Ink ink) {
            if (!string.IsNullOrWhiteSpace(ink.Color)) {
                var cielab = ColorHelper.ToCIELAB(ink.Color);
                ink.Color_CIELAB_L = cielab.L;
                ink.Color_CIELAB_a = cielab.A;
                ink.Color_CIELAB_b = cielab.B;
            }
        }

        // PUT: api/Inks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInk(int id, InkDTO inkDto)
        {
            var ink = _mapper.Map<Ink>(inkDto);
            if (ink == null || id != ink.Id) {
                return BadRequest();
            }
         
            FillCIELab(ink);
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
        [HttpPost]
        public async Task<ActionResult<Ink>> PostInk(InkDTO inkDto)
        {
            var ink = _mapper.Map<Ink>(inkDto);
            if (ink == null) {
                return BadRequest();
            }

            FillCIELab(ink);
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

            await _context.FountainPens
                .Where(x => x.CurrentInkId == ink.Id)
                .ForEachAsync(x => x.CurrentInk = null);

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
