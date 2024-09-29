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
using AutoMapper.QueryableExtensions;
using AutoMapper;
using FountainPensNg.Server.Helpers;

namespace FountainPensNg.Server.Controllers
{
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
        public async Task<ActionResult<IEnumerable<InkForListDTO>>> GetInks()
        {
            var query = _context
                .Inks
                .Include(x => x.CurrentPens.Take(1))
                .AsQueryable();
            return await query
                .ProjectTo<InkForListDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
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

        //todo: should not happen on the server!!! otherwise filter will be slow

        // [HttpGet("ByColor/{color}")]
        // public async Task<ActionResult<IEnumerable<InkForListDTO>>> GetInksByColor(string colorHex) {
        //     if (string.IsNullOrWhiteSpace(colorHex)) return NotFound();
        //     //get cielab from html
        //     var filterCielab = ColorHelper.ToCIELAB(colorHex);
        //     //get distances of all colors from filter color in cielab
        //     var filteredInks = new List<InkForListDTO>();
        //     var inks = await GetInks();
        //     if (inks == null) return NotFound();
        //     foreach (var ink in inks.Value) {
        //         if (!ink.Color_CIELAB_a.HasValue || !ink.Color_CIELAB_b.HasValue || !ink.Color_CIELAB_L.HasValue) continue;
        //         var inkCieLAB = new ColorHelper.CIELAB() {
        //             L = ink.Color_CIELAB_L.Value,
        //             A = ink.Color_CIELAB_a.Value,
        //             B = ink.Color_CIELAB_b.Value
        //         };
        //         var dist = ColorHelper.GetEuclideanDistance(filterCielab, inkCieLAB);
        //         if (dist < 50) { //dist between two blues: 5, orange->yellow: 35, b->w: 115, g->b: 258
        //             filteredInks.Add(ink);
        //         }
        //     }
        //     return filteredInks;
        // }

        private void FillCIELab(Ink ink) {
            if (!string.IsNullOrWhiteSpace(ink.Color)) {
                var cielab = ColorHelper.ToCIELAB(ink.Color);
                ink.Color_CIELAB_L = cielab.L;
                ink.Color_CIELAB_a = cielab.A;
                ink.Color_CIELAB_b = cielab.B;
            }
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ink>> PostInk(Ink ink)
        {
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
