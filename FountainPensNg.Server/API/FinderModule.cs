using Carter;
using FountainPensNg.Server.Data.Repos;
using FountainPensNg.Server.Migrations;

namespace FountainPensNg.Server.API {
    public class FinderModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/api/Finder", async (FinderRepo finderRepo) => 
                await finderRepo.FindAll(""))
                .WithTags("Finder");
            app.MapGet("/api/Finder/{fulltext}", async (string fulltext, FinderRepo finderRepo) =>
                await finderRepo.FindAll(fulltext))
                .WithTags("Finder");
        }
    }
}
