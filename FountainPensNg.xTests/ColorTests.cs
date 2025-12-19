namespace FountainPensNg.xTests;

[CollectionDefinition("Color Tests")]
public class ColorTestsCollection : ICollectionFixture<DbFixture>;

[Collection("Color Tests")]
public class ColorTests {
	private readonly DbFixture _fixture;
	public ColorTests(DbFixture fixture) {
		_fixture = fixture;
	}

	[Fact]
	public async Task GetCielchDistance() {
		var client = _fixture.Factory.CreateClient();
		var response = await client.GetAsync($"/api/colors/cie-lch-distance?color=%23FFFFFF");
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<float>();
		Assert.True(result > 300);
	}
}