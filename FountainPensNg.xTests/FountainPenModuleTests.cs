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

namespace FountainPensNg.xTests {

	public class FountainPenModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
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
		private async Task<FountainPen> GetFirst() {
			using var scope = factory.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
		 	return await context.FountainPens.FirstAsync();
		}

		[Fact]
		public async Task GetFountainPen() {
			await PrepareData();
			var pen = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.GetAsync($"/api/fountainpens/{pen.Id}");
			response.EnsureSuccessStatusCode();

			var fountainPens = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
			Assert.NotNull(fountainPens);
		}

		[Fact]
		public async Task GetFountainPens() {
			await PrepareData();
			var client = factory.CreateClient();

			var response = await client.GetAsync("/api/fountainpens");
			response.EnsureSuccessStatusCode();

			var fountainPens = await response.Content.ReadFromJsonAsync<IEnumerable<FountainPenDownloadDTO>>();
			Assert.NotNull(fountainPens);
			Assert.NotEmpty(fountainPens);
		}

		[Fact]
		public async Task UpdateFountainPen() {
			await PrepareData();
			var client = factory.CreateClient();
			var pen = await GetFirst();
			var dto = pen.Adapt<FountainPenUploadDTO>();
			var content = JsonContent.Create(dto);
			var response = await client.PutAsync($"/api/fountainpens/{dto.Id}", content);
			response.EnsureSuccessStatusCode();

			var fountainPens = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
			Assert.NotNull(fountainPens);
		}

		[Fact]
		public async Task DeleteFountainPen() {
			await PrepareData();
			var pen = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.DeleteAsync($"/api/fountainpens/{pen.Id}");
			response.EnsureSuccessStatusCode();
			Assert.True(true);
		}

		[Fact]
		public async Task AddFountainPen() {
			await PrepareData();
			var client = factory.CreateClient();
			var dto = new FountainPenUploadDTO(
				Id: 0, Maker: "Maker3", ModelName: "Model3", Comment: "test", Photo: "", Color: "#085172",
				Rating: 1, Nib: "F", CurrentInkId: null, CurrentInkRating: null, ImageObjectKey: "");
			var content = JsonContent.Create(dto);
			var response = await client.PostAsync($"/api/fountainpens", content);
			response.EnsureSuccessStatusCode();

			var fountainPens = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
			Assert.NotNull(fountainPens);
		}
	}
}