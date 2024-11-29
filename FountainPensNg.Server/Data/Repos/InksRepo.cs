using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Migrations;
using Humanizer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.FountainPensRepo;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.Data.Repos {
    public class InksRepo {
        private readonly DataContext _context;
        public InksRepo(DataContext context) {
            _context = context;
        }

        public async Task<IEnumerable<InkDownloadDTO>> GetInks() {
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
        }
        public async Task<InkDownloadDTO?> GetInk(int id) {
            var r = await _context
                .Inks
                .FindAsync(id);
            return r.Adapt<InkDownloadDTO>();
        }
        public record InkResult(ResultTypes ResultType, Ink? Ink = null);
        public async Task<InkResult> UpdateInk(int id, InkUploadDTO dto) {
            var ink = dto.Adapt<Ink>();
            if (ink == null || id != ink.Id) {
                return new InkResult(ResultTypes.BadRequest);
            }
            FillCIELab(ink);
            _context.Entry(ink).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new InkResult(ResultTypes.Ok, ink);
        }
        public async Task<InkResult> AddInk(InkUploadDTO dto) {
            var ink = dto.Adapt<Ink>();
            if (ink == null) return new InkResult(ResultTypes.BadRequest);
            FillCIELab(ink);
            _context.Inks.Add(ink);
            await _context.SaveChangesAsync();
            return new InkResult(ResultTypes.Ok, ink);
        }
        public async Task<InkResult> DeleteInk(int id) {
            //TODO: what if I delete the active inkedup?
            var inkedUp = await _context.Inks.FindAsync(id);
            if (inkedUp == null) return new InkResult(ResultTypes.NotFound);
            _context.Inks.Remove(inkedUp);
            await _context.SaveChangesAsync();
            return new InkResult(ResultTypes.Ok);
        }
        private void FillCIELab(Ink ink) {
            if (!string.IsNullOrWhiteSpace(ink.Color)) {
                var cielab = ColorHelper.ToCIELAB(ink.Color);
                ink.Color_CIELAB_L = cielab.L;
                ink.Color_CIELAB_a = cielab.A;
                ink.Color_CIELAB_b = cielab.B;
            }
        }
    }
}
