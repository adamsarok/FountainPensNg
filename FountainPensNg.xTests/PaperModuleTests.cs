[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace FountainPensNg.xTests;

[CollectionDefinition("Paper Tests")]
public class PaperCollection : ICollectionFixture<DbFixture>;

[Collection("Paper Tests")]
public class PaperModuleTests {
	private readonly DbFixture _fixture;
	public PaperModuleTests(DbFixture fixture) {
		_fixture = fixture;
	}

	[Fact]
	public async Task GetPaper() {
		var paper = _fixture.Papers.First();
		var client = _fixture.Factory.CreateClient();
		var response = await client.GetAsync($"/api/papers/{paper.Id}");
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<PaperDTO>();
		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetPapers() {
		var client = _fixture.Factory.CreateClient();

		var response = await client.GetAsync("/api/papers");
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<IEnumerable<PaperDTO>>();
		Assert.NotNull(result);
		Assert.NotEmpty(result);
	}

	[Fact]
	public async Task UpdatePaper() {
		var client = _fixture.Factory.CreateClient();
		var paper = _fixture.Papers.First();
		var dto = paper.Adapt<PaperDTO>();
		var content = JsonContent.Create(dto);
		var response = await client.PutAsync($"/api/papers/{dto.Id}", content);
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<PaperDTO>();
		Assert.NotNull(result);
	}

	[Fact]
	public async Task DeletePaper() {
		var paper = _fixture.Papers[1];
		var client = _fixture.Factory.CreateClient();
		var response = await client.DeleteAsync($"/api/papers/{paper.Id}");
		response.EnsureSuccessStatusCode();
		Assert.True(true);
	}

	[Fact]
	public async Task AddPaper() {
		var client = _fixture.Factory.CreateClient();
		var dto = new PaperDTO(
			Id: 0, Maker: "Maker3", PaperName: Guid.NewGuid().ToString(), Comment: "test", Photo: "",
			Rating: 1, ImageObjectKey: "", ImageUrl: "", CreatedAt: DateTime.UtcNow, UpdatedAt: DateTime.UtcNow);
		var content = JsonContent.Create(dto);
		var response = await client.PostAsync($"/api/papers", content);
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<PaperDTO>();
		Assert.NotNull(result);
	}
}