using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Exceptions;
using FountainPensNg.Server.Helpers;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.Data.Repos {
	public class FountainPensRepo(DataContext context) {
        public async Task<FountainPenDownloadDTO> GetFountainPen(int id) {
            var pen = await context.FountainPens
                .Include(fp => fp.InkedUps)
                .ThenInclude(iu => iu.Ink)
                .FirstOrDefaultAsync(f => f.Id == id);
			if (pen == null) throw new NotFoundException();
			return pen.Adapt<FountainPenDownloadDTO>(); //projectToType generates insanely complex query
		}

        public async Task<IEnumerable<FountainPenDownloadDTO>> GetFountainPens() {
            var r = await context
                .FountainPens
                .Include(x => x.InkedUps)
                .ThenInclude(inkup => inkup.Ink)
                .ToListAsync();
            return r.Adapt<List<FountainPenDownloadDTO>>(); //projectToType fails here
		}
        public bool FountainPenExists(int id) {
            return context.FountainPens.Any(e => e.Id == id);
        }
        public async Task<FountainPenDownloadDTO> UpdateFountainPen(int id, FountainPenUploadDTO dto) {
            var fountainPen = dto.Adapt<FountainPen>();
            if (fountainPen == null || id != fountainPen.Id) throw new NotFoundException();

            var find = await context.FountainPens.FindAsync(fountainPen.Id);
            if (find == null) throw new NotFoundException();
			context.Entry(find).CurrentValues.SetValues(fountainPen);
            find.ModifiedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return fountainPen.Adapt<FountainPenDownloadDTO>();
        }

        public async Task<FountainPenDownloadDTO> AddFountainPen(FountainPenUploadDTO dto) {
            var fountainPen = dto.Adapt<FountainPen>();
            if (fountainPen == null) throw new NotFoundException();
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
            return fountainPen.Adapt<FountainPenDownloadDTO>();
        }
        public async Task DeleteFountainPen(int id) {
            var fountainPen = await context.FountainPens.FindAsync(id);
            if (fountainPen == null) throw new NotFoundException();
			context.FountainPens.Remove(fountainPen);
            await context.SaveChangesAsync();
        }
    }
}
