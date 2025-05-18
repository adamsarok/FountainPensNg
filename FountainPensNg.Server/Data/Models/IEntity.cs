namespace FountainPensNg.Server.Data.Models;
public interface IEntity {
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }
}
