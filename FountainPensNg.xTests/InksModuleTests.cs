namespace FountainPensNg.xTests;
public class InksModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
	static bool dbUp = false;
	private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
	private async Task PrepareData() {          //fixtures don't have DI
		await semaphore.WaitAsync();
		try {
			if (!dbUp) {
				using var scope = factory.Services.CreateScope();
				using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
				var sql = "truncate table \"public\".\"Inks\" cascade";
				await context.Database.ExecuteSqlRawAsync(sql);
				context.Inks.AddRange(inksSeed);
				await context.SaveChangesAsync();
				dbUp = true;
			}
		} finally {
			semaphore.Release();
		}
	}
	static readonly List<Ink> inksSeed = new List<Ink> {
			new Ink { Id = 0, Maker = "Maker1", InkName = "InkForInkup1", Color = "#085172", Rating = 10
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("InkForInkup1")
				,Comment = "InkedUpModuleTests"
			},
			new Ink { Id = 0, Maker = "Maker2", InkName = "InkForInkup2", Color = "#d00606", Rating = 5
				,FullText = NpgsqlTypes.NpgsqlTsVector.Parse("InkForInkup2")
				,Comment = "InkedUpModuleTests"
			}
		};
	private async Task<Ink> GetFirst() {
		using var scope = factory.Services.CreateScope();
		using var context = scope.ServiceProvider.GetRequiredService<FountainPensContext>();
		return await context.Inks.FirstAsync();
	}

	[Fact]
	public async Task GetInk() {
		await PrepareData();
		var pen = await GetFirst();
		var client = factory.CreateClient();
		var response = await client.GetAsync($"/api/inks/{pen.Id}");
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
		var ink = await GetFirst();
		var dto = ink.Adapt<InkUploadDTO>();
		var content = JsonContent.Create(dto);
		var response = await client.PutAsync($"/api/inks/{dto.Id}", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task DeleteFountainPen() {
		await PrepareData();
		var pen = await GetFirst();
		var client = factory.CreateClient();
		var response = await client.DeleteAsync($"/api/inks/{pen.Id}");
		response.EnsureSuccessStatusCode();
		Assert.True(true);
	}

	[Fact]
	public async Task AddFountainPen() {
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