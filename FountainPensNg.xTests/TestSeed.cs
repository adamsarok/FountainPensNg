namespace FountainPensNg.xTests;
public class TestSeed {
	public List<FountainPen> FountainPens = new List<FountainPen> {
			new FountainPen { Id = 1, Maker = "Maker1", ModelName = "Model1", Color = "#085172", Nib = "F", Rating = 10
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Model1")
			},
			new FountainPen { Id = 2, Maker = "Maker2", ModelName = "Model2", Color = "#d00606", Nib = "M", Rating = 5
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Model2")
			}
		};
	public List<Ink> Inks = new List<Ink> {
			new Ink { Id = 1, Maker = "Maker1", InkName = "Ink1", Color = "#085172", Rating = 10
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Ink1")
			},
			new Ink { Id = 2, Maker = "Maker2", InkName = "Ink2", Color = "#d00606", Rating = 5
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Ink2")
			}
		};
	public List<Paper> Papers = new List<Paper> {
			new Paper { Id = 1, Maker = "Maker1", PaperName = "Paper1", Rating = 10
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Paper1")
			},
			new Paper { Id = 2, Maker = "Maker2", PaperName = "Paper2", Rating = 5
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("Paper2")
			}
		};
	public List<InkedUp> InkedUps = new List<InkedUp>();
	public async Task SeedInks(FountainPensContext context) {
		await TruncateInks(context);
		context.Inks.AddRange(Inks);
	}

	public async Task TruncateInks(FountainPensContext context) {
		var sql = "truncate table \"public\".\"Inks\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task SeedPapers(FountainPensContext context) {
		await TruncatePapers(context);
		context.Papers.AddRange(Papers);
	}

	public async Task TruncatePapers(FountainPensContext context) {
		var sql = "truncate table \"public\".\"Papers\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task SeedFountainPens(FountainPensContext context) {
		await TruncateFountainPens(context);
		context.FountainPens.AddRange(FountainPens);
	}

	public async Task TruncateFountainPens(FountainPensContext context) {
		var sql = "truncate table \"public\".\"FountainPens\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task SeedInkups(FountainPensContext context) {
		var sql = "truncate table \"public\".\"InkedUps\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
		InkedUps.Add(new InkedUp() { FountainPen = FountainPens[0], Ink = Inks[0], InkedAt = DateTime.UtcNow });
		InkedUps.Add(new InkedUp() { FountainPen = FountainPens[1], Ink = Inks[1], InkedAt = DateTime.UtcNow });
		context.InkedUps.AddRange(InkedUps);
	}
}