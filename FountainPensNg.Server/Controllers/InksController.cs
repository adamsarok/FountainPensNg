using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.DTO;

using FountainPensNg.Server.Helpers;
using Mapster;

namespace FountainPensNg.Server.Controllers {
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
        public async Task<ActionResult<IEnumerable<InkDownloadDTO>>> GetInks()
        {
            //TODO: this hack is most likely not needed with mapster
            var query = _context
                .Inks
                .Select(ink => new InkDownloadDTO(
                        ink.Id,
                        ink.Maker,
                        ink.InkName,
                        ink.Comment,
                        ink.Photo,
                        ink.Color,
                        ink.Color_CIELAB_L,
                        ink.Color_CIELAB_a,
                        ink.Color_CIELAB_b,
                        ink.Rating,
                        ink.Ml,
                        "",
                        "",
                        "",
                        ink.ImageObjectKey,
                        ink.InkedUps!
                            .Where(iu => iu.IsCurrent) // Apply filter here
                            .Select(iu => new InkedUpDTO(
                                iu.Id,
                                iu.InkedAt,
                                iu.MatchRating,
                                iu.FountainPenId,
                                iu.FountainPen.Maker,
                                iu.FountainPen.ModelName,
                                iu.Ink.Id,
                                iu.Ink.Maker,
                                iu.Ink.InkName,
                                iu.FountainPen.Color,
                                iu.Ink.Color)
                            )
                            .ToList() // Ensure the collection is materialized
                    ));
        
            var result = await query.ToListAsync();
#warning TODO OneCurrentPenMaker
            //foreach (var i in result) {
            //    if (i.InkedUpDTOs != null && i.InkedUpDTOs.Any()) {
            //        var pen = i.InkedUpDTOs.OrderByDescending(x => x.InkedAt).FirstOrDefault();
            //        if (pen != null) {
            //            i.OneCurrentPenMaker = pen.PenMaker;
            //            i.OneCurrentPenModelName = pen.PenName;
            //            i.OneCurrentPenColor = pen.PenColor;
            //        }
            //    }
            //}
            return result;
            // var query = _context
            //     .Inks
            //     .Include(x => x.InkedUps!.Where(iu => iu.IsCurrent))
            //     .ThenInclude(inkedup => inkedup.FountainPen) //TODO fill FP
            //     .AsQueryable();
            // return await query
            //     .ProjectTo<InkDTO>(_mapper.ConfigurationProvider)
            //     .ToListAsync();
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
        public async Task<IActionResult> PutInk(int id, InkUploadDTO inkDto)
        {
            var ink = inkDto.Adapt<Ink>();
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
        public async Task<ActionResult<Ink>> PostInk(InkUploadDTO inkDto)
        {
            var ink = inkDto.Adapt<Ink>();
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
