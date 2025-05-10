namespace FountainPensNg.Server.Data.DTO;
public record InkedUpDTO(
	 int Id,
	 DateTime InkedAt,
	 int MatchRating,
	 int FountainPenId,
	 string PenMaker,
	 string PenName,
	 int InkId,
	 string InkMaker,
	 string InkName,
	 string PenColor,
	 string InkColor,
	 string Comment);

