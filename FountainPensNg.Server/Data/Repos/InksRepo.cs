using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Exceptions;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Migrations;
using Humanizer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using static FountainPensNg.Server.Data.Repos.FountainPensRepo;
using static FountainPensNg.Server.Data.Repos.ResultType;
using static FountainPensNg.Server.Helpers.ColorHelper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FountainPensNg.Server.Data.Repos {
	public class InksRepo(FountainPensContext context) {
		private static InkDownloadDTO GetInkDownloadDTO(Ink ink) {
			var pen = ink.InkedUps?.FirstOrDefault(x => x.IsCurrent)?.FountainPen;
			return new InkDownloadDTO(
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
				pen?.Maker,
				pen?.ModelName,
				pen?.Color,
				ink.ImageObjectKey,
				ColorHelper.GetEuclideanDistanceToReference(ink.Color_CIELAB_L, ink.Color_CIELAB_a, ink.Color_CIELAB_b),
				ink.InkedUps?.Adapt<List<InkedUpDTO>>(),
				ink.InkedUps != null && ink.InkedUps.Any() ? ink.InkedUps.Max(x => x.InkedAt) : null
				);
		}

		public async Task<IEnumerable<InkDownloadDTO>> GetInks() {
			var inks = await context
				.Inks
				.Include(iu => iu.InkedUps)
				.ThenInclude(p => p.FountainPen)
				.ToListAsync();
			var result = new List<InkDownloadDTO>();
			foreach (var ink in inks) result.Add(GetInkDownloadDTO(ink));
			return result;
		}
		public async Task<InkDownloadDTO?> GetInk(int id) {
			var ink = await context
				.Inks
				.Include(iu => iu.InkedUps)
				.ThenInclude(p => p.FountainPen)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (ink == null) throw new NotFoundException();
			return GetInkDownloadDTO(ink);
		}
		public async Task<InkDownloadDTO> UpdateInk(int id, InkUploadDTO dto) {
			var ink = dto.Adapt<Ink>();
			if (ink == null || id != ink.Id) throw new NotFoundException();
			FillCIELab(ink);
			context.Entry(ink).State = EntityState.Modified;
			await context.SaveChangesAsync();
			return ink.Adapt<InkDownloadDTO>();
		}
		public async Task<InkDownloadDTO> AddInk(InkUploadDTO dto) {
			var ink = dto.Adapt<Ink>();
			if (ink == null) throw new MappingException();
			FillCIELab(ink);
			context.Inks.Add(ink);
			await context.SaveChangesAsync();
			return ink.Adapt<InkDownloadDTO>();
		}
		public async Task DeleteInk(int id) {
			//TODO: what if I delete the active inkedup?
			var inkedUp = await context.Inks.FindAsync(id);
			if (inkedUp == null) throw new NotFoundException();
			context.Inks.Remove(inkedUp);
			await context.SaveChangesAsync();
		}
		private static void FillCIELab(Ink ink) {
			if (!string.IsNullOrWhiteSpace(ink.Color)) {
				var cielab = ColorHelper.ToCIELAB(ink.Color);
				ink.Color_CIELAB_L = cielab.L;
				ink.Color_CIELAB_a = cielab.A;
				ink.Color_CIELAB_b = cielab.B;
			}
		}
	}
}
