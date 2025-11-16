namespace FountainPensNg.xTests;

[CollectionDefinition("Inks Tests")]
public class InksCollection : ICollectionFixture<DbFixture>;

[Collection("Inks Tests")]
public class InksModuleTests {
	private readonly DbFixture _fixture;
	public InksModuleTests(DbFixture fixture) {
		_fixture = fixture;
	}

	[Fact]
	public async Task GetInk() {
		var ink = _fixture.Inks.First();
		var client = _fixture.Factory.CreateClient();
		var response = await client.GetAsync($"/api/inks/{ink.Id}");
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetInks() {
		var client = _fixture.Factory.CreateClient();

		var response = await client.GetAsync("/api/inks");
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<IEnumerable<InkDownloadDTO>>();
		Assert.NotNull(result);
		Assert.NotEmpty(result);
	}

	[Fact]
	public async Task UpdateInk() {
		var client = _fixture.Factory.CreateClient();
		var ink = _fixture.Inks.First();
		var dto = ink.Adapt<InkUploadDTO>();
		var content = JsonContent.Create(dto);
		var response = await client.PutAsync($"/api/inks/{dto.Id}", content);
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
		Assert.NotNull(result);
	}

	[Fact]
	public async Task DeleteInk() {
		var ink = _fixture.Inks[2];
		var client = _fixture.Factory.CreateClient();
		var response = await client.DeleteAsync($"/api/inks/{ink.Id}");
		response.EnsureSuccessStatusCode();
		Assert.True(true);
	}

	[Fact]
	public async Task AddInk() {
		var client = _fixture.Factory.CreateClient();
		var dto = new InkUploadDTO(
			Id: 0, Maker: "Maker3", InkName: "Model3", Comment: "test", Photo: "", Color: "#085172",
			Rating: 1, Ml: 50, ImageObjectKey: "");
		var content = JsonContent.Create(dto);
		var response = await client.PostAsync($"/api/inks", content);
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
		Assert.NotNull(result);
	}
}