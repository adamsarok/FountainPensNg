using Carter;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Repos;

namespace FountainPensNg.Server.API {
	public class FountainPenModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/api/FountainPens", async (FountainPensRepo repo) =>
                await repo.GetFountainPens())
                .WithTags("FountainPens")
                .WithName("GetFountainPens");

            app.MapGet("/api/FountainPens/{id}", async (int id, FountainPensRepo repo) => {
                var fountainPen = await repo.GetFountainPen(id);
                return Results.Ok(fountainPen);
            }).WithTags("FountainPens")
                .WithName("GetFountainPen");

            app.MapPut("/api/FountainPens/{id}", async (int id, FountainPenUploadDTO dto, FountainPensRepo repo) => {
                var result = await repo.UpdateFountainPen(id, dto);
                return Results.Ok(result);
            }).WithTags("FountainPens")
                .WithName("PutFountainPen");

            app.MapPost("/api/FountainPens/", async (FountainPenUploadDTO dto, FountainPensRepo repo) => {
                var result = await repo.AddFountainPen(dto);
                return Results.CreatedAtRoute("GetFountainPen", new { id = result?.Id }, result);
            }).WithTags("FountainPens")
                .WithName("PostFountainPen");

            app.MapDelete("/api/FountainPens/{id}", async (int id, FountainPensRepo repo) => {
                await repo.DeleteFountainPen(id);
                return Results.NoContent();
            }).WithTags("FountainPens")
                .WithName("DeleteFountainPen");
        }
    }
}
