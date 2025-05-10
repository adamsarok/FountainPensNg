namespace FountainPensNg.xTests;

public class HealthTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
	[Fact]
	public async Task GetHealth() {
		var client = factory.CreateClient();
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