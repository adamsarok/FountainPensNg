using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Helpers;
using FountainPensNg.Server.Migrations;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using System.Net.Http.Json;

namespace FountainPensNg.xTests {

	public class InkedUpModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
		static bool dbUp = false;
		private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

		//this can be tricky if ink and fountainpen tests run in parallel and can affect this module
		private async Task PrepareData() {          //fixtures don't have DI
			await semaphore.WaitAsync();
			try {
				if (!dbUp) {
					using var scope = factory.Services.CreateScope();
					using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
					var sql = "truncate table \"public\".\"InkedUps\" cascade; truncate table \"public\".\"Inks\" cascade; truncate table \"public\".\"FountainPens\" cascade; ";
					await context.Database.ExecuteSqlRawAsync(sql);
					context.Inks.AddRange(TestSeed.Inks);
					context.FountainPens.AddRange(TestSeed.FountainPens);
					await context.SaveChangesAsync();
					var i = context.Inks.First();
					var p = context.FountainPens.First();
					context.InkedUps.AddRange(new InkedUp() { FountainPen = p, Ink = i, InkedAt = DateTime.UtcNow });
					await context.SaveChangesAsync();
					dbUp = true;
				}
			} finally {
				semaphore.Release();
			}
		}

		private async Task<InkedUp> GetFirst() {
			using var scope = factory.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
			return await context.InkedUps.FirstAsync();
		}

		[Fact]
		public async Task GetInkedUp() {
			await PrepareData();
			var inkedUp = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.GetAsync($"/api/InkedUps/{inkedUp.Id}");
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
			Assert.NotNull(inks);
		}

		[Fact]
		public async Task GetInkedUps() {
			await PrepareData();
			var client = factory.CreateClient();

			var response = await client.GetAsync("/api/InkedUps");
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<IEnumerable<InkedUpDTO>>();
			Assert.NotNull(inks);
			Assert.NotEmpty(inks);
		}

		[Fact]
		public async Task UpdateInkedUp() {
			await PrepareData();
			var client = factory.CreateClient();
			var ink = await GetFirst();
			var dto = ink.Adapt<InkedUpUploadDto>();
			var content = JsonContent.Create(dto);
			var response = await client.PutAsync($"/api/InkedUps/{dto.Id}", content);
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
			Assert.NotNull(inks);
		}

		[Fact]
		public async Task DeleteInkedUp() {
			await PrepareData();
			var inkedup = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.DeleteAsync($"/api/InkedUps/{inkedup.Id}");
			response.EnsureSuccessStatusCode();
			Assert.True(true);
		}

		[Fact]
		public async Task AddInkedUp() {
			await PrepareData();
			var client = factory.CreateClient();
			var example = await GetFirst();
			var dto = new InkedUpUploadDto(0, DateTime.UtcNow, 5, example.FountainPenId, example.InkId, "test");
			var content = JsonContent.Create(dto);
			var response = await client.PostAsync($"/api/InkedUps", content);
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<InkedUpDTO>();
			Assert.NotNull(inks);
		}
	}
}