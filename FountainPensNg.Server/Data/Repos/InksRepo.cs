using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Migrations;
using Humanizer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.FountainPensRepo;
using static FountainPensNg.Server.Data.Repos.ResultType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FountainPensNg.Server.Data.Repos {
    public class InksRepo {
        private readonly DataContext _context;
        public InksRepo(DataContext context) {
            _context = context;
        }

        private List<InkDownloadDTO> ConstructInkDownloadDTOs(List<Ink> inks) {
            List<InkDownloadDTO> result = new();
            foreach (var ink in inks) {
                var pen = ink.InkedUps.FirstOrDefault()?.FountainPen;
                result.Add(new InkDownloadDTO(
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
                    pen != null ? pen.Maker : "",
                    pen != null ? pen.ModelName : "",
                    pen != null ? pen.Color : "",
                    ink.ImageObjectKey,
                    ColorHelper.GetEuclideanDistanceToReference(ink.Color_CIELAB_L, ink.Color_CIELAB_a, ink.Color_CIELAB_b),
                    ink.InkedUps.Adapt<List<InkedUpDTO>>()
                    ));
            }
            return result;

        }

        public async Task<IEnumerable<InkDownloadDTO>> GetInks() {
            var inks = await _context
                .Inks
                .Include(iu => iu.InkedUps.Where(iu => iu.IsCurrent))
                .ThenInclude(p => p.FountainPen)
                .ToListAsync();
            return ConstructInkDownloadDTOs(inks);
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
