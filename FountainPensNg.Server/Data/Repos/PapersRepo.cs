using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Exceptions;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Migrations;
using FountainPensNg.Server.Services;
using Humanizer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.FountainPensRepo;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.Data.Repos {
	public class PapersRepo(FountainPensContext context, IPresignedUrlService presignedUrlService) {
		private PaperDTO GetPaperDownloadDTO(Paper paper) {
			return new PaperDTO(
				paper.Id,
				paper.Maker,
				paper.PaperName,
				paper.Comment,
				paper.Photo,
				paper.Rating,
				paper.ImageObjectKey,
				presignedUrlService.GetUrl(paper.ImageObjectKey, Amazon.S3.HttpVerb.GET),
				paper.CreatedAt,
				paper.UpdatedAt
			);
		}

		public async Task<IEnumerable<PaperDTO>> GetPapers() {
			var p = await context.Papers
				.ToListAsync();
			return p.Select(x => GetPaperDownloadDTO(x));
		}
		public async Task<PaperDTO?> GetPaper(int id) {
			var p = await context.Papers
				.FirstOrDefaultAsync(x => x.Id == id);
			return GetPaperDownloadDTO(p);
		}
		public async Task<PaperDTO> UpdatePaper(int id, PaperDTO dto) {
			var paper = dto.Adapt<Paper>();
			if (paper == null || id != paper.Id) throw new NotFoundException();
			context.Entry(paper).State = EntityState.Modified;
			await context.SaveChangesAsync();
			return GetPaperDownloadDTO(paper);
		}
		public async Task<PaperDTO> AddPaper(PaperDTO dto) {
			var paper = dto.Adapt<Paper>();
			if (paper == null) throw new MappingException();
			context.Papers.Add(paper);
			await context.SaveChangesAsync();
			return GetPaperDownloadDTO(paper);
		}
		public async Task DeletePaper(int id) {
			var paper = await context.Papers.FindAsync(id);
			if (paper == null) throw new NotFoundException();
			paper.IsDeleted = true;
			await context.SaveChangesAsync();
		}
	}
}
