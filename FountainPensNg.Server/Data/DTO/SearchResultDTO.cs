namespace FountainPensNg.Server.Data.DTO {
    public enum SearchResultTypes { Pen, Ink, Paper };
    public record SearchResultDTO(
        string SearchResultType,
        int Id,
        string Maker,
        string Model,
        string Comment,
        string Photo,
        string Color,
        int Rating,
        string ImageObjectKey,
        string ImageUrl);
}