namespace FountainPensNg.xTests;
public class RandomsFixture : IAsyncLifetime
{
    public WebApplicationFactory<Program> Factory { get; }
    public TestSeed TestSeed { get; } = new();

    public RandomsFixture()
    {
        Factory = new WebApplicationFactory<Program>();
    }

    public async Task InitializeAsync()
    {
		// Run once before all tests in this collection
		using var scope = Factory.Services.CreateScope();
		using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
		await TestSeed.SeedInks(context);
		await TestSeed.SeedFountainPens(context);
		await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
}

[CollectionDefinition("Randoms Tests")]
public class RandomsCollection : ICollectionFixture<RandomsFixture>
{
    // This class is just a marker for xUnit
}