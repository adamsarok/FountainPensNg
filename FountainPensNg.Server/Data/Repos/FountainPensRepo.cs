﻿using FountainPensNg.Server.Services;

namespace FountainPensNg.Server.Data.Repos;
public class FountainPensRepo(FountainPensContext context, IPresignedUrlService presignedUrlService) {
	private FountainPenDownloadDTO GetPenDownloadDTO(FountainPen pen) {
		var currentInkedUp = pen.InkedUps
			.Where(x => x.IsCurrent)
			//.Select(x => x.Ink)
			.FirstOrDefault();
		return new FountainPenDownloadDTO(
			pen.Id,
			pen.Maker,
			pen.ModelName,
			pen.Comment,
			pen.Photo,
			pen.Color,
			pen.Rating,
			pen.Nib,
			currentInkedUp?.Ink.Id,
			currentInkedUp?.MatchRating,
			currentInkedUp?.Comment,
			pen.ImageObjectKey,
			presignedUrlService.GetUrl(pen.ImageObjectKey, Amazon.S3.HttpVerb.GET),
			pen.InkedUps.Adapt<List<InkedUpDTO>>(),
			currentInkedUp?.Ink.Adapt<InkDownloadDTO>(),
			ColorHelper.GetEuclideanDistanceToReference(pen.Color),
			pen.CreatedAt,
			pen.UpdatedAt,
			pen.InkedUps != null && pen.InkedUps.Any() ? pen.InkedUps.Max(x => x.InkedAt) : null
		);
	}
	public async Task<FountainPenDownloadDTO> GetFountainPen(int id) {
		var pen = await context.FountainPens
			.Include(fp => fp.InkedUps)
			.ThenInclude(iu => iu.Ink)
			.FirstOrDefaultAsync(f => f.Id == id);
		if (pen == null) throw new NotFoundException();
		return GetPenDownloadDTO(pen);
	}
	public async Task<IEnumerable<FountainPenDownloadDTO>> GetFountainPens() {
		var pens = await context
			.FountainPens
			.Include(x => x.InkedUps)
			.ThenInclude(inkup => inkup.Ink)
			.ToListAsync();
		var result = new List<FountainPenDownloadDTO>();
		foreach (var pen in pens) result.Add(GetPenDownloadDTO(pen));
		return result;
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
		await context.SaveChangesAsync();
		return GetPenDownloadDTO(fountainPen);
	}

	public async Task<FountainPenDownloadDTO> AddFountainPen(FountainPenUploadDTO dto) {
		var fountainPen = dto.Adapt<FountainPen>();
		if (fountainPen == null) throw new MappingException();
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
		return GetPenDownloadDTO(fountainPen);
	}
	public async Task DeleteFountainPen(int id) {
		var fountainPen = await context.FountainPens.FindAsync(id);
		if (fountainPen == null) throw new NotFoundException();
		fountainPen.IsDeleted = true;
		await context.SaveChangesAsync();
	}
}