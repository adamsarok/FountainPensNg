using System.ComponentModel.DataAnnotations.Schema;

namespace FountainPensNg.Server.Data.Models {
    public class Ink {
        public int Id { get; set; }
        public string Maker { get; set; }
        public string InkName { get; set; }
        public string Comment { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Color { get; set; } = "";
        public double? Color_CIELAB_L { get; set; }
        public double? Color_CIELAB_a { get; set; }
        public double? Color_CIELAB_b { get; set; }
        public Statuses Status { get; set; }
        public enum Statuses { Favorite, OK, Bad }
        public virtual List<InkedUp> InkedUps { get; set; }
        public virtual List<FountainPen> CurrentPens { get; set; }

        [NotMapped]
        public string? PenDisplayName { get { return CurrentPens?.FirstOrDefault()?.PenDisplayName; } set {; } }
    }
}
