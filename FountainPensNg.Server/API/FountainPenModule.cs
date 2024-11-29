using Carter;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data.Repos;

namespace FountainPensNg.Server.API {
    public class FountainPenModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/api/FountainPens", async (FountainPensRepo repo) =>
                await repo.GetFountainPens())
                .WithTags("FountainPens");

            app.MapGet("/api/FountainPens/{id}", async (int id, FountainPensRepo repo) => {
                var fountainPen = await repo.GetFountainPen(id);
                if (fountainPen == null) return Results.NotFound();
                return Results.Ok(fountainPen);
            })
                .WithTags("FountainPens")
                .WithName("GetFountainPen");

            //TODO: there must be a better way to handle this without using HTTP results in the repo...
            app.MapPut("/api/FountainPens/{id}", async (int id, FountainPenUploadDTO dto, FountainPensRepo repo) => {
                var result = await repo.UpdateFountainPen(id, dto);
                switch (result.ResultType) {
                    case FountainPensRepo.ResultTypes.Ok: return Results.Ok(result.FountainPen);
                    case FountainPensRepo.ResultTypes.NotFound: return Results.NotFound();
                    case FountainPensRepo.ResultTypes.BadRequest:
                    default: return Results.InternalServerError();
                }
            })
                .WithTags("FountainPens");

            app.MapPost("/api/FountainPens/", async (FountainPenUploadDTO dto, FountainPensRepo repo) => {
                var result = await repo.AddFountainPen(dto);
                switch (result.ResultType) {
                    case FountainPensRepo.ResultTypes.Ok: 
                        return Results.CreatedAtRoute("GetFountainPen", new { id = result.FountainPen.Id }, result.FountainPen);
                    case FountainPensRepo.ResultTypes.NotFound: return Results.NotFound();
                    case FountainPensRepo.ResultTypes.BadRequest: return Results.BadRequest();
                    default: return Results.InternalServerError();
                }
            })
                .WithTags("FountainPens");

            app.MapDelete("/api/FountainPens/{id}", async (int id, FountainPensRepo repo) => {
                var result = await repo.DeleteFountainPen(id);
                switch (result.ResultType) {
                    case FountainPensRepo.ResultTypes.Ok: return Results.NoContent();
                    case FountainPensRepo.ResultTypes.NotFound: return Results.NotFound();
                    default: return Results.InternalServerError();
                }
            })
                .WithTags("FountainPens");
        }
    }
}
