namespace FountainPensNg.Server.Data.Models {
    public class InkedUp {
        public int Id { get; set; }
        public DateTime InkedAt { get; set; }
        public string Comment { get; set; } = "";
        public enum Rating { Perfect_Match, Good, Bad }
        public Rating MatchRating { get; set; }
        public virtual FountainPen FountainPen { get; set; }
        public virtual Ink Ink { get; set; }
    }
}
