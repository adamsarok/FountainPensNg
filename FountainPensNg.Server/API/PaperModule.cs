﻿namespace FountainPensNg.Server.API;
public class PaperModule : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        app.MapGet("/api/papers", async (PapersRepo repo) =>
            await repo.GetPapers())
            .WithTags("Papers")
            .WithName("GetPapers");

        app.MapGet("/api/papers/{id}", async (int id, PapersRepo repo) => {
            var ink = await repo.GetPaper(id);
            if (ink == null) return Results.NotFound();
            return Results.Ok(ink);
        }).WithTags("Papers")
            .WithName("GetPaper");

        app.MapPut("/api/papers/{id}", async (int id, PaperDTO dto, PapersRepo repo) => {
            var result = await repo.UpdatePaper(id, dto);
            return Results.Ok(result);
        }).WithTags("Papers")
            .WithName("PutPaper");

        app.MapPost("/api/papers/", async (PaperDTO dto, PapersRepo repo) => {
            var result = await repo.AddPaper(dto);
            return Results.CreatedAtRoute("GetPaper", new { id = result?.Id }, result);
        }).WithTags("Papers")
            .WithName("PostPaper");

        app.MapDelete("/api/papers/{id}", async (int id, PapersRepo repo) => {
            await repo.DeletePaper(id);
            return Results.NoContent();
        }).WithTags("Papers")
            .WithName("DeletePaper");
    }
}