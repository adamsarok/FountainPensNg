namespace FountainPensNg.Server.API;
public class InkedUpModule : ICarterModule {
    public void AddRoutes(IEndpointRouteBuilder app) {
        app.MapGet("/api/inked-ups", async (InkedUpsRepo repo) =>
            await repo.GetInkedUps())
            .WithTags("InkedUps")
            .WithName("GetInkedUps");

        app.MapGet("/api/inked-ups/{id}", async (int id, InkedUpsRepo repo) => {
            var inkedUp = await repo.GetInkedUp(id);
            return Results.Ok(inkedUp);
        }).WithTags("InkedUps")
            .WithName("GetInkedUp");

        //TODO: remove id from params where id is in DTO
        app.MapPut("/api/inked-ups/{id}", async (int id, InkedUpUploadDto dto, InkedUpsRepo repo) => {
            var result = await repo.UpdateInkedUp(dto);
            return Results.Ok(result);
        }).WithTags("InkedUps")
            .WithName("PutInkedUp");

        app.MapPost("/api/inked-ups/", async (InkedUpUploadDto dto, InkedUpsRepo repo) => {
            var result = await repo.AddInkedUp(dto);
            return Results.CreatedAtRoute("GetInkedUp", new { id = result?.Id }, result);
        }).WithTags("InkedUps")
            .WithName("PostInkedUp");

        app.MapDelete("/api/inked-ups/{id}", async (int id, InkedUpsRepo repo) => {
            await repo.DeleteInkedUp(id);
            return Results.NoContent();
        }).WithTags("InkedUps")
            .WithName("DeleteInkedUp");
    }
}