namespace FountainPensNg.Server.Data.DTO {
    public class SearchResultDTO {
        public enum SearhResultTypes { Pen, Ink, Paper };
        public string SearhResultType { get; set; }
        public int Id { get; set; }
        public string Maker { get; set; } = "";
        public string Model { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Photo { get; set; } = "";
        public string Color { get; set; } = "";
        public int Rating { get; set; }
        public int Ml { get; set; }
        public string ImageObjectKey { get; set; } = "";
    }
}
