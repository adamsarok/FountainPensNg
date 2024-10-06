using System.ComponentModel.DataAnnotations.Schema;
using FountainPensNg.Server.Data.DTO;

namespace FountainPensNg.Server.Data.Models {
    public class FountainPenUploadDTO {
        public int Id { get; set; }
        public string Maker { get; set; } = "";
        public string ModelName { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Color { get; set; } = "";
        public int Rating { get; set; }
        public string Nib { get; set; } = "";
        public int? CurrentInkId { get; set; }
        public int? CurrentInkRating { get; set; } 
        public string ImageObjectKey { get; set; } = "";
    }
    public class FountainPenDownloadDTO : FountainPenUploadDTO {
        public virtual List<InkedUpDTO>? InkedUps { get; set; } = new List<InkedUpDTO>();
        public virtual InkDTO? CurrentInk { get; set; }
        public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; }
    }
}
