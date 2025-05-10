namespace FountainPensNg.Server.Data.DTO;
public record FountainPenUploadDTO(int Id,
	string Maker,
	string ModelName,
	string Comment,
	string Photo,
	string Color,
	int Rating,
	string Nib,
	int? CurrentInkId,
	int? CurrentInkRating,
	string ImageObjectKey);
