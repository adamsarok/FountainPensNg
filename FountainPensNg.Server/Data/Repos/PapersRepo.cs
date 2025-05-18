using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Exceptions;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Migrations;
using Humanizer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.FountainPensRepo;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.Data.Repos {
	public class PapersRepo(FountainPensContext context) {
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
		public async Task<PaperDTO> UpdatePaper(int id, PaperDTO dto) {
			var paper = dto.Adapt<Paper>();
			if (paper == null || id != paper.Id) throw new NotFoundException();
			context.Entry(paper).State = EntityState.Modified;
			await context.SaveChangesAsync();
			return paper.Adapt<PaperDTO>();
		}
		public async Task<PaperDTO> AddPaper(PaperDTO dto) {
			var paper = dto.Adapt<Paper>();
			if (paper == null) throw new MappingException();
			context.Papers.Add(paper);
			await context.SaveChangesAsync();
			return paper.Adapt<PaperDTO>();
		}
		public async Task DeletePaper(int id) {
			var paper = await context.Papers.FindAsync(id);
			if (paper == null) throw new NotFoundException();
			paper.IsDeleted = true;
			await context.SaveChangesAsync();
		}
	}
}
