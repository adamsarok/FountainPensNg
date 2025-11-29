using Bogus;
using Microsoft.AspNetCore.Hosting;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace FountainPensNg.xTests;

public class DbFixture : IAsyncLifetime {
	public WebApplicationFactory<Program> Factory { get; }

	// Configuration properties for mock data generation
	public int FountainPenCount { get; set; } = 3;
	public int InkCount { get; set; } = 3;
	public int PaperCount { get; set; } = 2;
	public int InkedUpCount { get; set; } = 2;

	// Faker instances
	private readonly Faker _faker = new Faker();
	private readonly Faker<FountainPen> _fountainPenFaker;
	private readonly Faker<Ink> _inkFaker;
	private readonly Faker<Paper> _paperFaker;
	private readonly Faker<InkedUp> _inkedUpFaker;

	public DbFixture() {
		Factory = new WebApplicationFactory<Program>()
			  .WithWebHostBuilder(builder => {
				  builder.UseEnvironment("Test");
			  });

		// Configure Faker for FountainPen
		_fountainPenFaker = new Faker<FountainPen>()
			.RuleFor(p => p.Id, f => f.IndexFaker + 1)
			.RuleFor(p => p.Maker, f => f.Company.CompanyName())
			.RuleFor(p => p.ModelName, f => f.Commerce.ProductName())
			.RuleFor(p => p.Color, f => f.Internet.Color())
			.RuleFor(p => p.Nib, f => f.PickRandom("F", "M", "B", "EF", "BB"))
			.RuleFor(p => p.Rating, f => f.Random.Int(1, 10))
			.RuleFor(p => p.Comment, f => f.Lorem.Sentence())
			.RuleFor(p => p.Photo, f => f.Image.PicsumUrl())
			.RuleFor(p => p.ImageObjectKey, f => f.Random.Guid().ToString())
			.RuleFor(p => p.CreatedAt, f => DateTime.SpecifyKind(f.Date.Recent(365), DateTimeKind.Utc))
			.RuleFor(p => p.UpdatedAt, f => DateTime.SpecifyKind(f.Date.Recent(30), DateTimeKind.Utc))
			.RuleFor(p => p.FullText, (f, p) => NpgsqlTypes.NpgsqlTsVector.Parse(p.Maker + " " + p.ModelName));

		// Configure Faker for Ink
		_inkFaker = new Faker<Ink>()
			.RuleFor(i => i.Id, f => f.IndexFaker + 1)
			.RuleFor(i => i.Maker, f => f.Company.CompanyName())
			.RuleFor(i => i.InkName, f => f.Commerce.Color() + " " + f.Commerce.ProductMaterial())
			.RuleFor(i => i.Color, f => f.Internet.Color())
			.RuleFor(i => i.Rating, f => f.Random.Int(1, 10))
			.RuleFor(i => i.Comment, f => f.Lorem.Sentence())
			.RuleFor(i => i.Photo, f => f.Image.PicsumUrl())
			.RuleFor(i => i.Ml, f => f.Random.Int(10, 100))
			.RuleFor(i => i.Color_CIELAB_L, f => f.Random.Double(0, 100))
			.RuleFor(i => i.Color_CIELAB_a, f => f.Random.Double(-128, 127))
			.RuleFor(i => i.Color_CIELAB_b, f => f.Random.Double(-128, 127))
			.RuleFor(i => i.ImageObjectKey, f => f.Random.Guid().ToString())
			.RuleFor(i => i.CreatedAt, f => DateTime.SpecifyKind(f.Date.Recent(365), DateTimeKind.Utc))
			.RuleFor(i => i.UpdatedAt, f => DateTime.SpecifyKind(f.Date.Recent(30), DateTimeKind.Utc))
			.RuleFor(i => i.FullText, (f, i) => NpgsqlTypes.NpgsqlTsVector.Parse(i.Maker + " " + i.InkName));

		// Configure Faker for Paper
		_paperFaker = new Faker<Paper>()
			.RuleFor(p => p.Id, f => f.IndexFaker + 1)
			.RuleFor(p => p.Maker, f => f.Company.CompanyName())
			.RuleFor(p => p.PaperName, f => f.Commerce.ProductName() + " Paper")
			.RuleFor(p => p.Rating, f => f.Random.Int(1, 10))
			.RuleFor(p => p.Comment, f => f.Lorem.Sentence())
			.RuleFor(p => p.Photo, f => f.Image.PicsumUrl())
			.RuleFor(p => p.ImageObjectKey, f => f.Random.Guid().ToString())
			.RuleFor(p => p.CreatedAt, f => DateTime.SpecifyKind(f.Date.Recent(365), DateTimeKind.Utc))
			.RuleFor(p => p.UpdatedAt, f => DateTime.SpecifyKind(f.Date.Recent(30), DateTimeKind.Utc))
			.RuleFor(p => p.FullText, (f, p) => NpgsqlTypes.NpgsqlTsVector.Parse(p.Maker + " " + p.PaperName));

		// Configure Faker for InkedUp (will be configured after pens and inks are available)
		_inkedUpFaker = new Faker<InkedUp>()
			.RuleFor(iu => iu.Id, f => f.IndexFaker + 1)
			.RuleFor(iu => iu.InkedAt, f => DateTime.SpecifyKind(f.Date.Recent(30), DateTimeKind.Utc))
			.RuleFor(iu => iu.Comment, f => f.Lorem.Sentence())
			.RuleFor(iu => iu.MatchRating, f => f.Random.Int(1, 10))
			.RuleFor(iu => iu.IsCurrent, f => f.Random.Bool(0.3f)) // 30% chance of being current
			.RuleFor(iu => iu.CreatedAt, f => DateTime.SpecifyKind(f.Date.Recent(365), DateTimeKind.Utc))
			.RuleFor(iu => iu.UpdatedAt, f => DateTime.SpecifyKind(f.Date.Recent(30), DateTimeKind.Utc));
	}

	public async Task InitializeAsync() {
		// Run once before all tests in this collection
		using var scope = Factory.Services.CreateScope();
		using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();

		var connectionString = context.Database.GetConnectionString();
		var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
		var databaseName = builder.Database ?? string.Empty;

		if (string.IsNullOrEmpty(databaseName)) throw new Exception("Test database not configure");
		if (!databaseName.Contains("test", StringComparison.OrdinalIgnoreCase)) throw new Exception("Connected database is not a test database");

		await SeedInks(context);
		await SeedFountainPens(context);
		await SeedPapers(context);
		await context.SaveChangesAsync();
		await SeedInkups(context);
		await context.SaveChangesAsync();
	}

	public async Task DisposeAsync() {
		await Factory.DisposeAsync();
	}

	// Generated data properties (populated during seeding)
	public List<FountainPen> FountainPens { get; private set; } = new();
	public List<Ink> Inks { get; private set; } = new();
	public List<Paper> Papers { get; private set; } = new();
	public List<InkedUp> InkedUps { get; private set; } = new();

	public async Task SeedInks(FountainPensContext context) {
		await TruncateInks(context);
		Inks = _inkFaker.Generate(InkCount);
		context.Inks.AddRange(Inks);
	}

	public async Task TruncateInks(FountainPensContext context) {
		var sql = "truncate table \"public\".\"Inks\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task SeedPapers(FountainPensContext context) {
		await TruncatePapers(context);
		Papers = _paperFaker.Generate(PaperCount);
		context.Papers.AddRange(Papers);
	}

	public async Task TruncatePapers(FountainPensContext context) {
		var sql = "truncate table \"public\".\"Papers\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task SeedFountainPens(FountainPensContext context) {
		await TruncateFountainPens(context);
		FountainPens = _fountainPenFaker.Generate(FountainPenCount);
		context.FountainPens.AddRange(FountainPens);
	}

	public async Task TruncateFountainPens(FountainPensContext context) {
		var sql = "truncate table \"public\".\"FountainPens\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task TruncateInkups(FountainPensContext context) {
		var sql = "truncate table \"public\".\"InkedUps\" cascade;";
		await context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task SeedInkups(FountainPensContext context) {
		await TruncateInkups(context);

		var pens = await context.FountainPens.ToListAsync();
		var inks = await context.Inks.ToListAsync();

		// Generate InkedUps with actual pen and ink references
		InkedUps.Clear();
		for (int i = 0; i < Math.Min(InkedUpCount, Math.Min(pens.Count, inks.Count)); i++) {
			var inkedUp = new InkedUp {
				FountainPen = pens[i % pens.Count],
				Ink = inks[i % inks.Count],
				InkedAt = DateTime.SpecifyKind(_faker.Date.Recent(30), DateTimeKind.Utc),
				Comment = _faker.Lorem.Sentence(),
				MatchRating = _faker.Random.Int(1, 10),
				IsCurrent = _faker.Random.Bool(0.3f),
				CreatedAt = DateTime.SpecifyKind(_faker.Date.Recent(365), DateTimeKind.Utc),
				UpdatedAt = DateTime.SpecifyKind(_faker.Date.Recent(30), DateTimeKind.Utc)
			};
			InkedUps.Add(inkedUp);
		}

		context.InkedUps.AddRange(InkedUps);
	}
}