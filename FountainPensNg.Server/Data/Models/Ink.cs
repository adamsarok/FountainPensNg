using NpgsqlTypes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FountainPensNg.Server.Data.Models {
    public class Ink {
        public int Id { get; set; }
        public string Maker { get; set; } = "";
        public string InkName { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Color { get; set; } = "";
        public double? Color_CIELAB_L { get; set; }
        public double? Color_CIELAB_a { get; set; }
        public double? Color_CIELAB_b { get; set; }
        public int Rating { get; set; }
        public int Ml { get; set; }
        public string ImageObjectKey { get; set; } = "";
        public required NpgsqlTsVector FullText { get; set; }
        public virtual List<InkedUp>? InkedUps { get; set; }
        public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; }
    }
}
