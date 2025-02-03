using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FountainPensNg.xTests {

	public class IntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {

        [Fact]
        public async Task Test1() {
            var client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var inkedups = await context
                .InkedUps
                .Include(x => x.FountainPen)
                .Include(x => x.Ink)
                .ToListAsync();

        }


        [Fact]
        public void Mapster() {
            var fountainPen = new FountainPen {
                InkedUps =  {
                    new() { IsCurrent = true, Ink = new Ink { Id = 1, InkName = "Blue Ink" }, MatchRating = 5 },
                    new() { IsCurrent = false, Ink = new Ink { Id = 2, InkName = "Red Ink" }, MatchRating = 3 }
                }
            };

            //var config = MapsterConfig.RegisterMappings();
            //TypeAdapterConfig.GlobalSettings.Apply(config);

            // Manual mapping
            var dto = fountainPen.Adapt<FountainPenDownloadDTO>();

            Console.WriteLine(dto.CurrentInk?.InkName); // Should print "Blue Ink"
            Console.WriteLine(dto.CurrentInkId);     // Should print 1
            Console.WriteLine(dto.CurrentInkRating); // Should print 5
        }
    }
}