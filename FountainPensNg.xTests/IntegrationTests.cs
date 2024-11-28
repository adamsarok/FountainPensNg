using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Helpers;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FountainPensNg.xTests {

    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>> {
        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTests(WebApplicationFactory<Program> factory) {
            _factory = factory;
        }

        [Fact]
        public async Task Test1() {
            var client = _factory.CreateClient();

            using (var scope = _factory.Services.CreateScope()) {
                using (var context = scope.ServiceProvider.GetRequiredService<DataContext>()) {
                    var inkedups = await context
                        .InkedUps
                        .Include(x => x.FountainPen)
                        .Include(x => x.Ink)
                        .ToListAsync();
                }
            }
        }

        [Fact]
        public void Mapster() {
            var fountainPen = new FountainPen {
                InkedUps = new List<InkedUp>
    {
        new InkedUp { IsCurrent = true, Ink = new Ink { Id = 1, InkName = "Blue Ink" }, MatchRating = 5 },
        new InkedUp { IsCurrent = false, Ink = new Ink { Id = 2, InkName = "Red Ink" }, MatchRating = 3 }
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