using FountainPensNg.Server.Data;
using FountainPensNg.Server.Data.DTO;
using FountainPensNg.Server.Data.Models;
using FountainPensNg.Server.Helpers;
using Mapster;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using System.Net.Http.Json;

[assembly: CollectionBehavior(DisableTestParallelization = true)] //runs 3 times faster? also prevents race condition in DB
namespace FountainPensNg.xTests {
	public class PaperModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
		static bool dbUp = false;
		private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
		private async Task PrepareData() {          //fixtures don't have DI
			await semaphore.WaitAsync();
			try {
				if (!dbUp) {
					using var scope = factory.Services.CreateScope();
					using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
					var sql = "truncate table \"public\".\"Papers\" cascade";
					await context.Database.ExecuteSqlRawAsync(sql);
					context.Papers.AddRange(TestSeed.Papers);
					await context.SaveChangesAsync();
					dbUp = true;
				}
			} finally {
				semaphore.Release();
			}
		}
		private async Task<Paper> GetFirst() {
			using var scope = factory.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
		 	return await context.Papers.FirstAsync();
		}

		[Fact]
		public async Task GetPaper() {
			await PrepareData();
			var pen = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.GetAsync($"/api/Papers/{pen.Id}");
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
			Assert.NotNull(inks);
		}

		[Fact]
		public async Task GetPapers() {
			await PrepareData();
			var client = factory.CreateClient();

			var response = await client.GetAsync("/api/Papers");
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<IEnumerable<PaperDTO>>();
			Assert.NotNull(inks);
			Assert.NotEmpty(inks);
		}

		[Fact]
		public async Task UpdatePaper() {
			await PrepareData();
			var client = factory.CreateClient();
			var ink = await GetFirst();
			var dto = ink.Adapt<PaperDTO>();
			var content = JsonContent.Create(dto);
			var response = await client.PutAsync($"/api/Papers/{dto.Id}", content);
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<PaperDTO>();
			Assert.NotNull(inks);
		}

		[Fact]
		public async Task DeletePaper() {
			await PrepareData();
			var pen = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.DeleteAsync($"/api/Papers/{pen.Id}");
			response.EnsureSuccessStatusCode();
			Assert.True(true);
		}

		[Fact]
		public async Task AddPaper() {
			await PrepareData();
			var client = factory.CreateClient();
			var dto = new PaperDTO(
				Id: 0, Maker: "Maker3", PaperName: Guid.NewGuid().ToString(), Comment: "test", Photo: "",
				Rating: 1, ImageObjectKey: "", InsertedAt: DateTime.UtcNow, ModifiedAt: DateTime.UtcNow);
			var content = JsonContent.Create(dto);
			var response = await client.PostAsync($"/api/Papers", content);
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<PaperDTO>();
			Assert.NotNull(inks);
		}
	}
}