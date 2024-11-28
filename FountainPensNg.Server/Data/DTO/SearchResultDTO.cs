namespace FountainPensNg.Server.Data.DTO {
    public enum SearhResultTypes { Pen, Ink, Paper };
    public record SearchResultDTO(
        string SearhResultType,
        int Id,
        string Maker,
        string Model,
        string Comment,
        string Photo,
        string Color,
        int Rating,
        int Ml,
        string ImageObjectKey);
}
