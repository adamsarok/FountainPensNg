namespace FountainPensNg.Server.Data.Models {
    public class InkedUp {
        public int Id { get; set; }
        public DateTime InkedAt { get; set; }
        public string Comment { get; set; } = "";
        public int MatchRating { get; set; }
        public virtual FountainPen FountainPen { get; set; }
        public int FountainPenId { get; set; }
        public virtual Ink Ink { get; set; }
        public int InkId { get; set; }
    }
}
