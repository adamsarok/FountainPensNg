﻿namespace FountainPensNg.Server.Data.Models;
public class Entity : IEntity {
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	public bool IsDeleted { get; set; }
}
