namespace FountainPensNg.xTests;
public class PaperModuleFixture : IAsyncLifetime
{
    public WebApplicationFactory<Program> Factory { get; }
    public TestSeed TestSeed { get; } = new();

    public PaperModuleFixture()
    {
        Factory = new WebApplicationFactory<Program>();
    }

    public async Task InitializeAsync()
    {
        // Run once before all tests in this collection
        using var scope = Factory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
        
        await TestSeed.TruncatePapers(context);
        await TestSeed.SeedPapers(context);
        await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
}

[CollectionDefinition("Paper Tests")]
public class PaperCollection : ICollectionFixture<PaperModuleFixture>
{
    // This class is just a marker for xUnit
}