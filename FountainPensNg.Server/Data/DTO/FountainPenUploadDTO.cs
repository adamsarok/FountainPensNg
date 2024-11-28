using System.ComponentModel.DataAnnotations.Schema;

namespace FountainPensNg.Server.Data.Models {
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
}
