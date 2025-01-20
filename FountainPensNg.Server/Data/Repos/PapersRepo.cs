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
    public class PapersRepo(DataContext context) {
        public async Task<IEnumerable<PaperDTO>> GetPapers() {
            return await context.Papers
                .ProjectToType<PaperDTO>()
                .ToListAsync();
        }
        public async Task<PaperDTO?> GetPaper(int id) {
            var r = await context.Papers
                .FirstOrDefaultAsync(x => x.Id == id);
            return r.Adapt<PaperDTO>();
        }
        public record PaperResult(ResultTypes ResultType, PaperDTO Paper = null);
        public async Task<PaperResult> UpdatePaper(int id, PaperDTO dto) {
            var paper = dto.Adapt<Paper>();
            if (paper == null || id != paper.Id) {
                return new PaperResult(ResultTypes.BadRequest);
            }
                context.Entry(paper).State = EntityState.Modified;
            paper.ModifiedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return new PaperResult(ResultTypes.Ok, paper.Adapt<PaperDTO>());
        }
        public async Task<PaperResult> AddPaper(PaperDTO dto) {
            var Paper = dto.Adapt<Paper>();
            if (Paper == null) return new PaperResult(ResultTypes.BadRequest);
            context.Papers.Add(Paper);
            await context.SaveChangesAsync();
            return new PaperResult(ResultTypes.Ok, Paper.Adapt<PaperDTO>());
        }
        public async Task<PaperResult> DeletePaper(int id) {
            var Paper = await context.Papers.FindAsync(id);
            if (Paper == null) return new PaperResult(ResultTypes.NotFound);
            context.Papers.Remove(Paper);
            await context.SaveChangesAsync();
            return new PaperResult(ResultTypes.Ok);
        }
    }
}
