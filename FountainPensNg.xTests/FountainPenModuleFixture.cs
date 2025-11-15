namespace FountainPensNg.xTests;
public class FountainPenModuleFixture : IAsyncLifetime
{
    public WebApplicationFactory<Program> Factory { get; }
    public TestSeed TestSeed { get; } = new();

    public FountainPenModuleFixture()
    {
        Factory = new WebApplicationFactory<Program>();
    }

    public async Task InitializeAsync()
    {
        // Run once before all tests in this collection
        using var scope = Factory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
        
        await TestSeed.TruncateFountainPens(context);
        await TestSeed.SeedFountainPens(context);
        await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
}

[CollectionDefinition("FountainPen Tests")]
public class FountainPenCollection : ICollectionFixture<FountainPenModuleFixture>
{
    // This class is just a marker for xUnit
}