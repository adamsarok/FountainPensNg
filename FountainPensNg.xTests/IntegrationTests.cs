using FountainPensNg.Server.Data;
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
    }
}