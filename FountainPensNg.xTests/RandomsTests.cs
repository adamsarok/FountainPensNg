namespace FountainPensNg.xTests;

[CollectionDefinition("Randoms Tests")]
public class RandomsCollection : ICollectionFixture<InkedUpModuleFixture>;

[Collection("Randoms Tests")]
public class RandomsTests {
	private readonly InkedUpModuleFixture _fixture;
	public RandomsTests(InkedUpModuleFixture fixture) {
		_fixture = fixture;
	}
	[Fact]
	public async Task GetRandoms() {
		var client = _fixture.Factory.CreateClient();
		var response = await client.GetAsync($"/api/randoms/{3}");
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<IEnumerable<InkedUpSuggestion>>();
		Assert.NotNull(result);
	}
}