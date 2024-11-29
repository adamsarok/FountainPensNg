using Carter;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.Repos;
using static FountainPensNg.Server.Data.Repos.ResultType;

namespace FountainPensNg.Server.API {
    public class InkedUpModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/api/InkedUps", async (InkedUpsRepo repo) =>
                await repo.GetInkedUps())
                .WithTags("InkedUps")
                .WithName("GetInkedUps");

            app.MapGet("/api/InkedUps/{id}", async (int id, InkedUpsRepo repo) => {
                var inkedUp = await repo.GetInkedUp(id);
                if (inkedUp == null) return Results.NotFound();
                return Results.Ok(inkedUp);
            }).WithTags("InkedUps")
                .WithName("GetInkedUp");

            app.MapPut("/api/InkedUps/{id}", async (int id, InkedUpDTO dto, InkedUpsRepo repo) => {
                var result = await repo.UpdateInkedUp(id, dto);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.Ok(result.InkedUp),
                    ResultTypes.NotFound => Results.NotFound(),
                    ResultTypes.BadRequest => Results.BadRequest(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("InkedUps")
                .WithName("PutInkedUp");

            app.MapPost("/api/InkedUps/", async (InkedUpDTO dto, InkedUpsRepo repo) => {
                var result = await repo.AddInkedUp(dto);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.CreatedAtRoute("GetInkedUp", new { id = result?.InkedUp?.Id }, result?.InkedUp),
                    ResultTypes.NotFound => Results.NotFound(),
                    ResultTypes.BadRequest => Results.BadRequest(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("InkedUps")
                .WithName("PostInkedUp");

            app.MapDelete("/api/InkedUps/{id}", async (int id, InkedUpsRepo repo) => {
                var result = await repo.DeleteInkedUp(id);
                return result.ResultType switch {
                    ResultTypes.Ok => Results.NoContent(),
                    ResultTypes.NotFound => Results.NotFound(),
                    _ => Results.InternalServerError()
                };
            }).WithTags("InkedUps")
                .WithName("DeleteInkedUp");
        }
    }
}
