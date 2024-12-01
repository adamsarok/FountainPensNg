using FountainPensNg.Server.Data.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FountainPensNg.Server.Data.DTO {
    public record InkDownloadDTO(
         int Id,
         string Maker,
         string InkName,
         string Comment,
         string Photo,
         string Color,
         double? Color_CIELAB_L,
         double? Color_CIELAB_a,
         double? Color_CIELAB_b,
         int Rating,
         int Ml,
         string? OneCurrentPenMaker,
         string? OneCurrentPenModelName,
         string? OneCurrentPenColor,
         string ImageObjectKey,
         double? CieLch_sort,
         List<InkedUpDTO> InkedUpDTOs);
}
