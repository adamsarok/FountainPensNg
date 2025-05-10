namespace FountainPensNg.Server.Data.Models;
public class InkedUp {
    public int Id { get; set; }
    public DateTime InkedAt { get; set; } = DateTime.UtcNow;
    public string Comment { get; set; } = "";
    public int MatchRating { get; set; }
    public required virtual FountainPen FountainPen { get; set; }
    public int FountainPenId { get; set; }
    public required virtual Ink Ink { get; set; }
    public int InkId { get; set; }
    public bool IsCurrent { get; set; } = true; //TODO: make sure only 1 ink can be current per pen
}