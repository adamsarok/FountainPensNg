namespace FountainPensNg.Server.Data.Models;
public class FountainPen : Entity {
    public int Id { get; set; }
    public string Maker { get; set; } = "";
    public string ModelName { get; set; } = "";
    public string Comment { get; set; } = "";
    public string Photo { get; set; } = "";
    public string Color { get; set; } = "";
    public int Rating { get; set; }
    public string Nib { get; set; } = "";
    public virtual List<InkedUp> InkedUps { get; set; } = [];
    public string ImageObjectKey { get; set; } = "";
    public required NpgsqlTsVector FullText { get; set; }
    public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; }
}