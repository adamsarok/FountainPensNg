using Carter;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.Repos;
using Microsoft.OpenApi.Models;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.API {
    public class PaperModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/api/Papers", async (PapersRepo repo) =>
                await repo.GetPapers())
                .WithTags("Papers")
                .WithName("GetPapers");

            app.MapGet("/api/Papers/{id}", async (int id, PapersRepo repo) => {
                var ink = await repo.GetPaper(id);
                if (ink == null) return Results.NotFound();
                return Results.Ok(ink);
            }).WithTags("Papers")
                .WithName("GetPaper");               

            app.MapPut("/api/Papers/{id}", async (int id, PaperDTO dto, PapersRepo repo) => {
                var result = await repo.UpdatePaper(id, dto);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.Ok(result.Paper),
                    ResultTypes.NotFound => Results.NotFound(),
                    ResultTypes.BadRequest => Results.BadRequest(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("Papers")
                .WithName("PutPaper");

            app.MapPost("/api/Papers/", async (PaperDTO dto, PapersRepo repo) => {
                var result = await repo.AddPaper(dto);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.CreatedAtRoute("GetInk", new { id = result?.Paper?.Id }, result?.Paper),
                    ResultTypes.NotFound => Results.NotFound(),
                    ResultTypes.BadRequest => Results.BadRequest(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("Papers")
                .WithName("PostPaper");

            app.MapDelete("/api/Papers/{id}", async (int id, PapersRepo repo) => {
                var result = await repo.DeletePaper(id);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.NoContent(),
                    ResultTypes.NotFound => Results.NotFound(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("Papers")
                .WithName("DeletePaper");
        }
    }
}
