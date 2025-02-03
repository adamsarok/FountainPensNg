using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.Data.Repos {
	public class FountainPensRepo(DataContext context) {

        //public async Task<FountainPen> GetFountainPen(int id) {
        //    return await _context.FountainPens
        //        .Include(pen => pen.InkedUps)
        //        .ThenInclude(inkup => inkup.Ink)
        //        .Where(x => x.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<FountainPenDownloadDTO> GetFountainPen(int id) {
            var pen = await context.FountainPens
                .Include(fp => fp.InkedUps)
                .ThenInclude(iu => iu.Ink)
                //.ProjectToType<FountainPenDownloadDTO>() //TODO: projectto generates insanely complex query
                .FirstOrDefaultAsync(f => f.Id == id);
            return pen.Adapt<FountainPenDownloadDTO>();
        }

        public async Task<IEnumerable<FountainPenDownloadDTO>> GetFountainPens() {
            var r = await context
                .FountainPens
                .Include(x => x.InkedUps)
                .ThenInclude(inkup => inkup.Ink)
                //.ProjectToType<FountainPenDownloadDTO>() //projectToType seems really finicky... this mapping works perfectly with adapt
                .ToListAsync();
            return r.Adapt<List<FountainPenDownloadDTO>>();
        }
        public bool FountainPenExists(int id) {
            return context.FountainPens.Any(e => e.Id == id);
        }
        public record FountainPenResult(ResultTypes ResultType, FountainPenDownloadDTO? FountainPen = null);
        public async Task<FountainPenResult> UpdateFountainPen(int id, FountainPenUploadDTO dto) {
            var fountainPen = dto.Adapt<FountainPen>();
            if (fountainPen == null || id != fountainPen.Id) {
                return new FountainPenResult(ResultTypes.BadRequest);
            }

            var find = await context.FountainPens.FindAsync(fountainPen.Id);
            if (find == null) return new FountainPenResult(ResultTypes.NotFound);

            context.Entry(find).CurrentValues.SetValues(fountainPen);
            find.ModifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return new FountainPenResult(ResultTypes.Ok, fountainPen.Adapt<FountainPenDownloadDTO>());
        }
        public async Task<FountainPenResult> AddFountainPen(FountainPenUploadDTO dto) {
            var fountainPen = dto.Adapt<FountainPen>();
            if (fountainPen == null) return new FountainPenResult(ResultTypes.BadRequest);
            context.FountainPens.Add(fountainPen);
            //TODO: auto add inkedup 
            // if (fountainPen.CurrentInkId.HasValue && fountainPen.CurrentInkId > 0) {
            //     _context.InkedUps.Add(new InkedUp() {
            //         FountainPen = fountainPen,
            //         InkId = fountainPen.CurrentInkId.Value,
            //         MatchRating = fountainPen.CurrentInkRating ?? 0
            //     });
            // }
            await context.SaveChangesAsync();
            return new FountainPenResult(ResultTypes.Ok, fountainPen.Adapt<FountainPenDownloadDTO>());
        }
        public async Task<FountainPenResult> DeleteFountainPen(int id) {
            var fountainPen = await context.FountainPens.FindAsync(id);
            if (fountainPen == null) return new FountainPenResult(ResultTypes.NotFound);
            context.FountainPens.Remove(fountainPen);
            await context.SaveChangesAsync();
            return new FountainPenResult(ResultTypes.Ok);
        }
    }
}
