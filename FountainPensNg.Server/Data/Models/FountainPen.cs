using System.ComponentModel.DataAnnotations.Schema;

namespace FountainPensNg.Server.Data.Models {
    public class FountainPen {
        public int Id { get; set; }
        public string Maker { get; set; }
        public string ModelName { get; set; }
        public string Comment { get; set; } = "";
        public Statuses Status { get; set; } = Statuses.OK;
        public string Photo { get; set; } = "";
        public string Color { get; set; } = "";
        public enum Statuses { Favorite, OK, Bad }
        public NibTypes Nib { get; set; } = NibTypes.M;
        public enum NibTypes { EF, F, M, B, S_1_1 }
        public virtual List<InkedUp> InkedUps { get; set; }
        public virtual Ink? CurrentInk { get; set; }

        [NotMapped]
        public string InkColor { get; set; }

        [NotMapped]
        public string PenDisplayName { get { return $"{Maker} {ModelName}"; } set {; } }

    }
}
