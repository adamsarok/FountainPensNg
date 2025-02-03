namespace FountainPensNg.Server.Data.DTO {
	public record FountainPenDownloadDTO(
		int Id,
		string Maker,
		string ModelName,
		string Comment,
		string Photo,
		string Color,
		int Rating,
		string Nib,
		int? CurrentInkId,
		int? CurrentInkRating,
		string ImageObjectKey,
		List<InkedUpDTO>? InkedUps,
		InkDownloadDTO? CurrentInk,
		DateTime InsertedAt,
		DateTime ModifiedAt);
}
