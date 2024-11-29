using FountainPensNg.Server.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FountainPensNg.Server.Data.Repos {
    public class FountainPensRepo {
        private readonly DataContext _context;

        public FountainPensRepo(DataContext context) {
            _context = context;
        }

        //public async Task<FountainPen> GetFountainPen(int id) {
        //    return await _context.FountainPens
        //        .Include(pen => pen.InkedUps)
        //        .ThenInclude(inkup => inkup.Ink)
        //        .Where(x => x.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<FountainPenDownloadDTO> GetFountainPen(int id) {
            var pen = await _context.FountainPens
                .Include(fp => fp.InkedUps)
                .ThenInclude(iu => iu.Ink)
                //.ProjectToType<FountainPenDownloadDTO>() //TODO: projectto generates insanely complex query
                .FirstOrDefaultAsync(f => f.Id == id);
            return pen.Adapt<FountainPenDownloadDTO>();
        }

        public async Task<IEnumerable<FountainPenDownloadDTO>> GetFountainPens() {
            var r = await _context
                .FountainPens
                .Include(x => x.InkedUps)
                .ThenInclude(inkup => inkup.Ink)
                //.ProjectToType<FountainPenDownloadDTO>() //projectToType seems really finicky... this mapping works perfectly with adapt
                .ToListAsync();
            return r.Adapt<List<FountainPenDownloadDTO>>();
        }
        public bool FountainPenExists(int id) {
            return _context.FountainPens.Any(e => e.Id == id);
        }
        public enum ResultTypes { Ok, NotFound, BadRequest }
        public record FountainPenResult(ResultTypes ResultType, FountainPen? FountainPen = null);
        public async Task<FountainPenResult> UpdateFountainPen(int id, FountainPenUploadDTO dto) {
            var fountainPen = dto.Adapt<FountainPen>();
            if (fountainPen == null || id != fountainPen.Id) {
                return new FountainPenResult(ResultTypes.BadRequest);
            }
            //TODO: it is a bad idea to add HTTP result to a DB repo, however this is simplest for now

            var find = await _context.FountainPens.FindAsync(fountainPen.Id);
            if (find == null) return new FountainPenResult(ResultTypes.NotFound);

            // Update the fountain pen properties
            _context.Entry(find).CurrentValues.SetValues(fountainPen);
            find.ModifiedAt = DateTime.UtcNow;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!FountainPenExists(id)) {
                    return new FountainPenResult(ResultTypes.NotFound);
                } else {
                    throw;
                }
            }

            return new FountainPenResult(ResultTypes.Ok, fountainPen);
        }
        public async Task<FountainPenResult> AddFountainPen(FountainPenUploadDTO dto) {
            var fountainPen = dto.Adapt<FountainPen>();
            if (fountainPen == null) return new FountainPenResult(ResultTypes.BadRequest);
            _context.FountainPens.Add(fountainPen);
            //TODO: auto add inkedup 
            // if (fountainPen.CurrentInkId.HasValue && fountainPen.CurrentInkId > 0) {
            //     _context.InkedUps.Add(new InkedUp() {
            //         FountainPen = fountainPen,
            //         InkId = fountainPen.CurrentInkId.Value,
            //         MatchRating = fountainPen.CurrentInkRating ?? 0
            //     });
            // }
            await _context.SaveChangesAsync();
            return new FountainPenResult(ResultTypes.Ok, fountainPen);
        }
        public async Task<FountainPenResult> DeleteFountainPen(int id) {
            var fountainPen = await _context.FountainPens.FindAsync(id);
            if (fountainPen == null) return new FountainPenResult(ResultTypes.NotFound);
            _context.FountainPens.Remove(fountainPen);
            await _context.SaveChangesAsync();
            return new FountainPenResult(ResultTypes.Ok);
        }
    }
}
