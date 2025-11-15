[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace FountainPensNg.xTests;
public class PaperModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
	static bool dbUp = false;
	private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
	private TestSeed TestSeed => new();
	private async Task PrepareData() {          //fixtures don't have DI
		await semaphore.WaitAsync();
		try {
			if (!dbUp) {
				using var scope = factory.Services.CreateScope();
				using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
				await TestSeed.SeedPapers(context);
				await context.SaveChangesAsync();
				dbUp = true;
			}
		} finally {
			semaphore.Release();
		}
	}

	[Fact]
	public async Task GetPaper() {
		await PrepareData();
		var paper = TestSeed.Papers.First();
		var client = factory.CreateClient();
		var response = await client.GetAsync($"/api/papers/{paper.Id}");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task GetPapers() {
		await PrepareData();
		var client = factory.CreateClient();

		var response = await client.GetAsync("/api/papers");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<IEnumerable<PaperDTO>>();
		Assert.NotNull(inks);
		Assert.NotEmpty(inks);
	}

	[Fact]
	public async Task UpdatePaper() {
		await PrepareData();
		var client = factory.CreateClient();
		var paper = TestSeed.Papers.First();
		var dto = paper.Adapt<PaperDTO>();
		var content = JsonContent.Create(dto);
		var response = await client.PutAsync($"/api/papers/{dto.Id}", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<PaperDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task DeletePaper() {
		await PrepareData();
		var paper = TestSeed.Papers[1];
		var client = factory.CreateClient();
		var response = await client.DeleteAsync($"/api/papers/{paper.Id}");
		response.EnsureSuccessStatusCode();
		Assert.True(true);
	}

	[Fact]
	public async Task AddPaper() {
		await PrepareData();
		var client = factory.CreateClient();
		var dto = new PaperDTO(
			Id: 0, Maker: "Maker3", PaperName: Guid.NewGuid().ToString(), Comment: "test", Photo: "",
			Rating: 1, ImageObjectKey: "", ImageUrl: "", CreatedAt: DateTime.UtcNow, UpdatedAt: DateTime.UtcNow);
		var content = JsonContent.Create(dto);
		var response = await client.PostAsync($"/api/papers", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<PaperDTO>();
		Assert.NotNull(inks);
	}
}