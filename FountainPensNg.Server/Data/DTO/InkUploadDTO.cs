namespace FountainPensNg.Server.Data.DTO;
public record InkUploadDTO(
 int Id,
 string Maker,
 string InkName,
 string Comment,
 string Photo,
 string Color,
 int Rating,
 int Ml,
 string ImageObjectKey);
