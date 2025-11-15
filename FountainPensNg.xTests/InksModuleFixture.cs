namespace FountainPensNg.xTests;
public class InksModuleFixture : IAsyncLifetime
{
    public WebApplicationFactory<Program> Factory { get; }
    public TestSeed TestSeed { get; } = new();

    public InksModuleFixture()
    {
        Factory = new WebApplicationFactory<Program>();
    }

    public async Task InitializeAsync()
    {
        // Run once before all tests in this collection
        using var scope = Factory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
        
        await TestSeed.TruncateInks(context);
        await TestSeed.SeedInks(context);
        await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
}

[CollectionDefinition("Inks Tests")]
public class InksCollection : ICollectionFixture<InksModuleFixture>
{
    // This class is just a marker for xUnit
}