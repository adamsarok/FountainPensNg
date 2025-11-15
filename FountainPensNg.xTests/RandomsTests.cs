namespace FountainPensNg.xTests;
public class RandomsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
	static bool dbUp = false;
	private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
	private TestSeed TestSeed => new();
	private async Task PrepareData() {
		await semaphore.WaitAsync();
		try {
			if (!dbUp) {
				using var scope = factory.Services.CreateScope();
				using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
				await TestSeed.SeedInks(context);
				await TestSeed.SeedFountainPens(context);
				await context.SaveChangesAsync();
				dbUp = true;
			}
		} finally {
			semaphore.Release();
		}
	}

	[Fact]
	public async Task GetRandoms() {
		await PrepareData();
		var client = factory.CreateClient();
		var response = await client.GetAsync($"/api/randoms/{3}");
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<IEnumerable<InkedUpSuggestion>>();
		Assert.NotNull(result);
	}
}