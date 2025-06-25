using FountainPensNg.Server.Data;
using FountainPensNg.Server.Services;
using System.Reflection.Metadata;

namespace FountainPensNg.Server.API;
public class FountainPenModule : ICarterModule {
	public void AddRoutes(IEndpointRouteBuilder app) {
		app.MapGet("/api/fountain-pens", async (FountainPensRepo repo) =>
			await repo.GetFountainPens())
			.WithTags("FountainPens")
			.WithName("GetFountainPens");

		app.MapGet("/api/fountain-pens/{id}", async (int id, FountainPensRepo repo) => {
			var fountainPen = await repo.GetFountainPen(id);
			return Results.Ok(fountainPen);
		}).WithTags("FountainPens")
			.WithName("GetFountainPen");

		app.MapPut("/api/fountain-pens/{id}", async (int id, FountainPenUploadDTO dto, FountainPensRepo repo) => {
			var result = await repo.UpdateFountainPen(id, dto);
			return Results.Ok(result);
		}).WithTags("FountainPens")
			.WithName("PutFountainPen");

		app.MapPost("/api/fountain-pens/", async (FountainPenUploadDTO dto, FountainPensRepo repo) => {
			var result = await repo.AddFountainPen(dto);
			return Results.CreatedAtRoute("GetFountainPen", new { id = result?.Id }, result);
		}).WithTags("FountainPens")
			.WithName("PostFountainPen");

		app.MapDelete("/api/fountain-pens/{id}", async (int id, FountainPensRepo repo) => {
			await repo.DeleteFountainPen(id);
			return Results.NoContent();
		}).WithTags("FountainPens")
			.WithName("DeleteFountainPen");

		app.MapPut("/api/fountain-pens/empty/{id}", async (int id, InkedUpsRepo repo) => {
			await repo.DeactivateInkedUps(id);
			return Results.NoContent();
		}).WithTags("FountainPens")
			.WithName("EmptyFountainPen");
	}
}