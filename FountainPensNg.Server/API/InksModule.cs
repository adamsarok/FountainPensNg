using Carter;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.Repos;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.API {
    public class InksModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/api/Inks", async (InksRepo repo) =>
                await repo.GetInks())
                .WithTags("Inks")
                .WithName("GetInks");

            app.MapGet("/api/Inks/{id}", async (int id, InksRepo repo) => {
                var ink = await repo.GetInk(id);
                if (ink == null) return Results.NotFound();
                return Results.Ok(ink);
            }).WithTags("Inks")
                .WithName("GetInk");

            app.MapPut("/api/Inks/{id}", async (int id, InkUploadDTO dto, InksRepo repo) => {
                var result = await repo.UpdateInk(id, dto);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.Ok(result.Ink),
                    ResultTypes.NotFound => Results.NotFound(),
                    ResultTypes.BadRequest => Results.BadRequest(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("Inks")
                .WithName("PutInk");

            app.MapPost("/api/Inks/", async (InkUploadDTO dto, InksRepo repo) => {
                var result = await repo.AddInk(dto);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.CreatedAtRoute("GetInk", new { id = result?.Ink?.Id }, result?.Ink),
                    ResultTypes.NotFound => Results.NotFound(),
                    ResultTypes.BadRequest => Results.BadRequest(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("Inks")
                .WithName("PostInk");

            app.MapDelete("/api/Inks/{id}", async (int id, InksRepo repo) => {
                var result = await repo.DeleteInk(id);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.NoContent(),
                    ResultTypes.NotFound => Results.NotFound(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("Inks")
                .WithName("DeleteInk");
        }
    }
}
