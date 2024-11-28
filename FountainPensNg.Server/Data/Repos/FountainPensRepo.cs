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
                .FirstOrDefaultAsync();
            return pen.Adapt<FountainPenDownloadDTO>();
        }
    }
}
