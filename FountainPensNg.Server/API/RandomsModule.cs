
namespace FountainPensNg.Server.API;

public class RandomsModule : ICarterModule {
	public void AddRoutes(IEndpointRouteBuilder app) {
		app.MapGet("/api/randoms/{count}", async (int count, RandomsRepo repo) =>
		  await repo.Get(count))
		  .WithTags("Randoms")
		  .WithName("GetRandoms");

	}
}
