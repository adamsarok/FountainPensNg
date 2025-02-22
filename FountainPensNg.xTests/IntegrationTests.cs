using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Helpers;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static FountainPensNg.Server.Helpers.ColorHelper;

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
        public void Colors() {
            var cielab = ColorHelper.ToCIELAB("#73D13E");
            Assert.Equal(-52, Math.Truncate(cielab.A));
			Assert.Equal(60, Math.Truncate(cielab.B));
			Assert.Equal(75, Math.Truncate(cielab.L));
		}
    }
}