namespace FountainPensNg.Server.Data.Models;
public class Paper : Entity {
    public int Id { get; set; }
    public string Maker { get; set; } = "";
    public string PaperName { get; set; } = "";
    public string Comment { get; set; } = "";
    public string Photo { get; set; } = "";
    public int Rating { get; set; }
    public string ImageObjectKey { get; set; } = "";
    public required NpgsqlTsVector FullText { get; set; }
}