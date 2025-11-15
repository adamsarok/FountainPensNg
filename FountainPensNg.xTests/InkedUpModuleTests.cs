namespace FountainPensNg.xTests;

[Collection("InkedUp Tests")]
public class InkedUpModuleTests {
	private readonly InkedUpModuleFixture _fixture;
	public InkedUpModuleTests(InkedUpModuleFixture fixture) {
		_fixture = fixture;
	}

	[Fact]
	public async Task GetInkedUp() {
		var inkedUp = _fixture.TestSeed.InkedUps.First();
		var client = _fixture.Factory.CreateClient();
		var response = await client.GetAsync($"/api/inked-ups/{inkedUp.Id}");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task GetInkedUps() {
		var client = _fixture.Factory.CreateClient();

		var response = await client.GetAsync("/api/inked-ups");
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<IEnumerable<InkedUpDTO>>();
		Assert.NotNull(inks);
		Assert.NotEmpty(inks);
	}

	[Fact]
	public async Task UpdateInkedUp() {
		var client = _fixture.Factory.CreateClient();
		var inkedUp = _fixture.TestSeed.InkedUps.First();
		var dto = inkedUp.Adapt<InkedUpUploadDto>();
		var content = JsonContent.Create(dto);
		var response = await client.PutAsync($"/api/inked-ups/{dto.Id}", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
		Assert.NotNull(inks);
	}

	[Fact]
	public async Task DeleteInkedUp() {
		var inkedup = _fixture.TestSeed.InkedUps[1];
		var client = _fixture.Factory.CreateClient();
		var response = await client.DeleteAsync($"/api/inked-ups/{inkedup.Id}");
		response.EnsureSuccessStatusCode();
		Assert.True(true);
	}

	[Fact]
	public async Task AddInkedUp() {
		var client = _fixture.Factory.CreateClient();
		var example = _fixture.TestSeed.InkedUps.First();
		var dto = new InkedUpUploadDto(0, DateTime.UtcNow, 5, example.FountainPenId, example.InkId, "test");
		var content = JsonContent.Create(dto);
		var response = await client.PostAsync($"/api/inked-ups", content);
		response.EnsureSuccessStatusCode();

		var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
		Assert.NotNull(inks);
	}
}