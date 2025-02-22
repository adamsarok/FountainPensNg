using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Helpers;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FountainPensNg.xTests {

	public class IntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {

        [Fact (Skip = "Placeholder/example on DI")]
        public async Task DBContext() {
            var client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var inkedups = await context
                .InkedUps
                .Include(x => x.FountainPen)
                .Include(x => x.Ink)
                .ToListAsync();

        }
    }
}