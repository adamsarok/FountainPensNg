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

	public class InksModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
		static bool dbUp = false;
		private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
		private async Task PrepareData() {          //fixtures don't have DI
			await semaphore.WaitAsync();
			try {
				if (!dbUp) {
					using var scope = factory.Services.CreateScope();
					using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
					var sql = "truncate table \"public\".\"Inks\" cascade";
					await context.Database.ExecuteSqlRawAsync(sql);
					context.Inks.AddRange(TestSeed.Inks);
					await context.SaveChangesAsync();
					dbUp = true;
				}
			} finally {
				semaphore.Release();
			}
		}
		private async Task<Ink> GetFirst() {
			using var scope = factory.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
		 	return await context.Inks.FirstAsync();
		}

		[Fact]
		public async Task GetInk() {
			await PrepareData();
			var pen = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.GetAsync($"/api/Inks/{pen.Id}");
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
			Assert.NotNull(inks);
		}

		[Fact]
		public async Task GetInks() {
			await PrepareData();
			var client = factory.CreateClient();

			var response = await client.GetAsync("/api/Inks");
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<IEnumerable<InkDownloadDTO>>();
			Assert.NotNull(inks);
			Assert.NotEmpty(inks);
		}

		[Fact]
		public async Task UpdateInk() {
			await PrepareData();
			var client = factory.CreateClient();
			var ink = await GetFirst();
			var dto = ink.Adapt<InkUploadDTO>();
			var content = JsonContent.Create(dto);
			var response = await client.PutAsync($"/api/Inks/{dto.Id}", content);
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
			Assert.NotNull(inks);
		}

		[Fact]
		public async Task DeleteFountainPen() {
			await PrepareData();
			var pen = await GetFirst();
			var client = factory.CreateClient();
			var response = await client.DeleteAsync($"/api/Inks/{pen.Id}");
			response.EnsureSuccessStatusCode();
			Assert.True(true);
		}

		[Fact]
		public async Task AddFountainPen() {
			await PrepareData();
			var client = factory.CreateClient();
			var dto = new InkUploadDTO(
				Id: 0, Maker: "Maker3", InkName: "Model3", Comment: "test", Photo: "", Color: "#085172",
				Rating: 1, Ml: 50, ImageObjectKey: "");
			var content = JsonContent.Create(dto);
			var response = await client.PostAsync($"/api/Inks", content);
			response.EnsureSuccessStatusCode();

			var inks = await response.Content.ReadFromJsonAsync<InkDownloadDTO>();
			Assert.NotNull(inks);
		}
	}
}