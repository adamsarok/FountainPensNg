namespace FountainPensNg.Server.Data.DTO;
public record PaperDTO(
	 int Id,
	 string Maker,
	 string PaperName,
	 string Comment,
	 string Photo,
	 int Rating,
	 string ImageObjectKey,
	 DateTime InsertedAt,
	 DateTime ModifiedAt);
