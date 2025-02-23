using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FountainPensNg.xTests {
	public class FinderPenModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
		static bool dbUp = false;
		private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
		private async Task PrepareData() {          //fixtures don't have DI
			await semaphore.WaitAsync();
			try {
				if (!dbUp) {
					using var scope = factory.Services.CreateScope();
					using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
					var sql = "truncate table \"public\".\"FountainPens\" cascade";
					await context.Database.ExecuteSqlRawAsync(sql);
					context.FountainPens.AddRange(TestSeed.FountainPens);
					await context.SaveChangesAsync();
					dbUp = true;
				}
			} finally {
				semaphore.Release();
			}
		}

		[Fact]
		public async Task GetFullText() {
			await PrepareData();
			var client = factory.CreateClient();
			var response = await client.GetAsync($"/api/Finder/{TestSeed.FountainPens[0].ModelName}");
			response.EnsureSuccessStatusCode();

			var fountainPens = await response.Content.ReadFromJsonAsync<IEnumerable<SearchResultDTO>>();
			Assert.NotNull(fountainPens);
		}

		[Fact]
		public async Task GetFindAll() {
			await PrepareData();
			var client = factory.CreateClient();

			var response = await client.GetAsync("/api/Finder");
			response.EnsureSuccessStatusCode();

			var fountainPens = await response.Content.ReadFromJsonAsync<IEnumerable<SearchResultDTO>>();
			Assert.NotNull(fountainPens);
			Assert.NotEmpty(fountainPens);
		}

	}
}
