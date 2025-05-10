namespace FountainPensNg.Server.Data.DTO;
public record InkedUpUploadDto(
	 int Id,
	 DateTime InkedAt,
	 int MatchRating,
	 int FountainPenId,
	 int InkId,
	 string Comment);

