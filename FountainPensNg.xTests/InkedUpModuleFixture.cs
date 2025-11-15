namespace FountainPensNg.xTests;
public class InkedUpModuleFixture : IAsyncLifetime
{
    public WebApplicationFactory<Program> Factory { get; }
    public TestSeed TestSeed { get; } = new();

    public InkedUpModuleFixture()
    {
        Factory = new WebApplicationFactory<Program>();
    }

    public async Task InitializeAsync()
    {
        // Run once before all tests in this collection
        using var scope = Factory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
        
        await TestSeed.TruncateInks(context);
        await TestSeed.TruncateFountainPens(context);
        await TestSeed.SeedInkups(context);
        await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
}

[CollectionDefinition("InkedUp Tests")]
public class InkedUpCollection : ICollectionFixture<InkedUpModuleFixture>
{
    // This class is just a marker for xUnit
}