namespace FountainPensNg.xTests;
public class InksModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
	static bool dbUp = false;
	private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
	private TestSeed TestSeed => new();
	private async Task PrepareData() {          //fixtures don't have DI
		await semaphore.WaitAsync();
		try {
			if (!dbUp) {
				using var scope = factory.Services.CreateScope();
				using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
				await TestSeed.SeedInks(context);
				await context.SaveChangesAsync();
				dbUp = true;
			}
		} finally {
			semaphore.Release();
		}
	}

	[Fact]
	public async Task GetInk() {
		await PrepareData();
		var ink = TestSeed.Inks.First();
		var client = factory.CreateClient();
		var response = await client.GetAsync($"/api/inks/{ink.Id}");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task GetInks() {
		await PrepareData();
		var client = factory.CreateClient();

		var response = await client.GetAsync("/api/inks");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<IEnumerable<InkDownloadDTO>>();
		Assert.NotNull(inks);
		Assert.NotEmpty(inks);
	}

	[Fact]
	public async Task UpdateInk() {
		await PrepareData();
		var client = factory.CreateClient();
		var ink = TestSeed.Inks.First();
		var dto = ink.Adapt<InkUploadDTO>();
		var content = JsonContent.Create(dto);
		var response = await client.PutAsync($"/api/inks/{dto.Id}", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task DeleteInk() {
		await PrepareData();
		var ink = TestSeed.Inks[1];
		var client = factory.CreateClient();
		var response = await client.DeleteAsync($"/api/inks/{ink.Id}");
		response.EnsureSuccessStatusCode();
		Assert.True(true);
	}

	[Fact]
	public async Task AddInk() {
		await PrepareData();
		var client = factory.CreateClient();
		var dto = new InkUploadDTO(
			Id: 0, Maker: "Maker3", InkName: "Model3", Comment: "test", Photo: "", Color: "#085172",
			Rating: 1, Ml: 50, ImageObjectKey: "");
		var content = JsonContent.Create(dto);
		var response = await client.PostAsync($"/api/inks", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
		Assert.NotNull(inks);
	}
}