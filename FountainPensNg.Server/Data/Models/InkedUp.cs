namespace FountainPensNg.Server.Data.Models {
    public class InkedUp {
        public int Id { get; set; }
        public DateTime InkedAt { get; set; } = DateTime.UtcNow;
        public string Comment { get; set; } = "";
        public int MatchRating { get; set; }
        public virtual FountainPen FountainPen { get; set; } = new FountainPen();
        public int FountainPenId { get; set; }
        public virtual Ink Ink { get; set; } = new Ink();
        public int InkId { get; set; }
        public bool IsCurrent { get; set; } = true; //TODO: create repo, make sure only 1 ink can be current per pen
    }
}
