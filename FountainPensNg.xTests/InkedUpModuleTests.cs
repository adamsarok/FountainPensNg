namespace FountainPensNg.xTests;
public class InkedUpModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
	static bool dbUp = false;
	private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
	private TestSeed TestSeed => new();
	//this can be tricky if ink and fountainpen tests run in parallel and can affect this module
	private async Task PrepareData() {          //fixtures don't have DI
		await semaphore.WaitAsync();
		try {
			if (!dbUp) {
				using var scope = factory.Services.CreateScope();
				using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
				await TestSeed.TruncateInks(context);
				await TestSeed.TruncateFountainPens(context);
				await TestSeed.SeedInkups(context);
				await context.SaveChangesAsync();
				dbUp = true;
			}
		} finally {
			semaphore.Release();
		}
	}

	[Fact]
	public async Task GetInkedUp() {
		await PrepareData();
		var inkedUp = TestSeed.InkedUps.First();
		var client = factory.CreateClient();
		var response = await client.GetAsync($"/api/inked-ups/{inkedUp.Id}");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task GetInkedUps() {
		await PrepareData();
		var client = factory.CreateClient();

		var response = await client.GetAsync("/api/inked-ups");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<IEnumerable<InkedUpDTO>>();
		Assert.NotNull(inks);
		Assert.NotEmpty(inks);
	}

	[Fact]
	public async Task UpdateInkedUp() {
		await PrepareData();
		var client = factory.CreateClient();
		var inkedUp = TestSeed.InkedUps.First();
		var dto = inkedUp.Adapt<InkedUpUploadDto>();
		var content = JsonContent.Create(dto);
		var response = await client.PutAsync($"/api/inked-ups/{dto.Id}", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task DeleteInkedUp() {
		await PrepareData();
		var inkedup = TestSeed.InkedUps[1];
		var client = factory.CreateClient();
		var response = await client.DeleteAsync($"/api/inked-ups/{inkedup.Id}");
		response.EnsureSuccessStatusCode();
		Assert.True(true);
	}

	[Fact]
	public async Task AddInkedUp() {
		await PrepareData();
		var client = factory.CreateClient();
		var example = TestSeed.InkedUps.First();
		var dto = new InkedUpUploadDto(0, DateTime.UtcNow, 5, example.FountainPenId, example.InkId, "test");
		var content = JsonContent.Create(dto);
		var response = await client.PostAsync($"/api/inked-ups", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
		Assert.NotNull(inks);
	}
}