namespace FountainPensNg.xTests;

[CollectionDefinition("Randoms Tests")]
public class RandomsCollection : ICollectionFixture<DbFixture>;

[Collection("Randoms Tests")]
public class RandomsTests {
	private readonly DbFixture _fixture;
	public RandomsTests(DbFixture fixture) {
		_fixture = fixture;
	}
	[Fact]
	public async Task GetRandoms() {
		//Arrange
		//Act
		var client = _fixture.Factory.CreateClient();
		var response = await client.GetAsync($"/api/randoms/{5}");
		//Assert
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<IEnumerable<InkedUpSuggestion>>();
		Assert.NotNull(result);
		Assert.NotEmpty(result);
		Assert.All(result, suggestion => {
			Assert.True(suggestion.FountainPenId > 0);
			Assert.True(suggestion.InkId > 0);
			Assert.NotNull(suggestion.PenMaker);
			Assert.NotNull(suggestion.InkMaker);
		});
	}
}