using NpgsqlTypes;

namespace FountainPensNg.Server.Data.DTO {
    public record PaperDTO(
         int Id,
         string Maker,
         string PaperName,
         string Comment,
         string Photo,
         int Rating,
         string ImageObjectKey,
         NpgsqlTsVector? FullText,
         DateTime InsertedAt,
         DateTime ModifiedAt);
}
