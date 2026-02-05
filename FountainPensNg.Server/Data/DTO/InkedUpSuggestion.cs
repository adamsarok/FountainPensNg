namespace FountainPensNg.Server.Data.DTO;
public record InkedUpSuggestion(
	 int FountainPenId,
	 string PenMaker,
	 string PenName,
	 int InkId,
	 string InkMaker,
	 string InkName,
	 string PenColor,
	 string InkColor,
	 string PenNib,
	 DateTime? InkLastInkedAt);
