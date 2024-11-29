using Carter;
using FountainPensNg.Server.Data.Repos;
using FountainPensNg.Server.Migrations;

namespace FountainPensNg.Server.API {
    public class FinderModule : ICarterModule {
        public void AddRoutes(IEndpointRouteBuilder app) {
            app.MapGet("/Finder", async (FinderRepo finderRepo) => 
                await finderRepo.FindAll(""))
                .WithTags("Finder");
            app.MapGet("/Finder/{fulltext}", async (string fulltext, FinderRepo finderRepo) => 
                await finderRepo.FindAll(fulltext))
                .WithTags("Finder");
        }
    }
}
