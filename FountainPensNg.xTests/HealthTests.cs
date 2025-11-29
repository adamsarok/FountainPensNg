namespace FountainPensNg.xTests;

[CollectionDefinition("Health Tests")]
public class HealthCollection : ICollectionFixture<DbFixture>;

[Collection("Health Tests")]
public class HealthTests {
	private readonly DbFixture _fixture;
	public HealthTests(DbFixture fixture) {
		_fixture = fixture;
	}

	[Fact]
	public async Task GetHealth() {
		var client = _fixture.Factory.CreateClient();
		var response = await client.GetAsync($"/api/health");
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<HealthCheckDto>();
		Assert.NotNull(result);
		Assert.Equal("Healthy", result.status);
		Assert.Equal("Healthy", result.entries.npgsql.status);
	}

	record Data();
	record Entries(
		Npgsql npgsql
	);
	record Npgsql(
		Data data,
		string description,
		string duration,
		string exception,
		string status,
		IReadOnlyList<object> tags
	);
	record HealthCheckDto(
		string status,
		string totalDuration,
		Entries entries
	);
}