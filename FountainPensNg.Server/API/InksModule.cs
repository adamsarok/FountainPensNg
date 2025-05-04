using Carter;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.Repos;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.API {
    public class InksModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/api/inks", async (InksRepo repo) =>
                await repo.GetInks())
                .WithTags("Inks")
                .WithName("GetInks");

            app.MapGet("/api/inks/{id}", async (int id, InksRepo repo) => {
                var ink = await repo.GetInk(id);
                return Results.Ok(ink);
            }).WithTags("Inks")
                .WithName("GetInk");

            app.MapPut("/api/inks/{id}", async (int id, InkUploadDTO dto, InksRepo repo) => {
                var result = await repo.UpdateInk(id, dto);
                return Results.Ok(result);
            }).WithTags("Inks")
                .WithName("PutInk");

            app.MapPost("/api/inks/", async (InkUploadDTO dto, InksRepo repo) => {
                var result = await repo.AddInk(dto);
                return Results.CreatedAtRoute("GetInk", new { id = result?.Id }, result);
            }).WithTags("Inks")
                .WithName("PostInk");

            app.MapDelete("/api/inks/{id}", async (int id, InksRepo repo) => {
                await repo.DeleteInk(id);
                return Results.NoContent();
            }).WithTags("Inks")
                .WithName("DeleteInk");
        }
    }
}
