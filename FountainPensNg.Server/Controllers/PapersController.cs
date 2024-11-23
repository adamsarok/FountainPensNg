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
    public class PapersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PapersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaperDTO>>> GetPapers()
        {
            return await _context
                .Papers
                .ProjectTo<PaperDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaperDTO>> GetPaper(int id) {
            var ink = await _context.Papers.FindAsync(id);

            if (ink == null) {
                return NotFound();
            }

            return _mapper.Map<PaperDTO>(ink);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaper(int id, PaperDTO dto)
        {
            var paper = _mapper.Map<Paper>(dto);
            if (paper == null || id != paper.Id) {
                return BadRequest();
            }
         
            _context.Entry(paper).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaperExists(id))
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


        [HttpPost]
        public async Task<ActionResult<PaperDTO>> PostInk(PaperDTO dto)
        {
            var paper = _mapper.Map<Paper>(dto);
            if (paper == null) {
                return BadRequest();
            }

            _context.Papers.Add(paper);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaper", new { id = paper.Id }, paper);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInk(int id)
        {
            var paper = await _context.Papers.FindAsync(id);
            if (paper == null)
            {
                return NotFound();
            }

            _context.Papers.Remove(paper);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaperExists(int id)
        {
            return _context.Papers.Any(e => e.Id == id);
        }
    }
}
