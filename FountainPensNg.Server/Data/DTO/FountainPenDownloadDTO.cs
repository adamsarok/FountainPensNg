using FountainPensNg.Server.Data.DTO;

namespace FountainPensNg.Server.Data.Models {
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
