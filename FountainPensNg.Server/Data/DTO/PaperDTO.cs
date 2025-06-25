namespace FountainPensNg.Server.Data.DTO;
public record PaperDTO(
	 int Id,
	 string Maker,
	 string PaperName,
	 string Comment,
	 string Photo,
	 int Rating,
	 string ImageObjectKey,
	 string ImageUrl,
	 DateTime CreatedAt,
	 DateTime UpdatedAt);
