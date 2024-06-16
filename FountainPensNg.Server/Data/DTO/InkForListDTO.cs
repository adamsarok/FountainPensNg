using FountainPensNg.Server.Data.Models;

namespace FountainPensNg.Server.Data.DTO {
    public class InkForListDTO {
        public int Id { get; set; }
        public string Maker { get; set; }
        public string InkName { get; set; }
        public string Comment { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Color { get; set; } = "";
        public double? Color_CIELAB_L { get; set; }
        public double? Color_CIELAB_a { get; set; }
        public double? Color_CIELAB_b { get; set; }
        public int Rating { get; set; }
        public FountainPen? OneCurrentPen { get; set; } 
    }
}
