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

	public class InkedUpModuleTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
		static bool dbUp = false;
		private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

		//TODO: this can be tricky as both ink and fountainpen tests run in parallel and can affect this module

		//private async Task PrepareData() {          //fixtures don't have DI
		//	await semaphore.WaitAsync();
		//	try {
		//		if (!dbUp) {
		//			using var scope = factory.Services.CreateScope();
		//			using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
		//			var sql = "truncate table \"public\".\"InkedUps\" cascade";
		//			await context.Database.ExecuteSqlRawAsync(sql);
		//			context.Inks.AddRange(TestSeed.Inks);
		//			await context.SaveChangesAsync();
		//			dbUp = true;
		//		}
		//	} finally {
		//		semaphore.Release();
		//	}
		//}

		//private async Task<Ink> GetFirst() {
		//	using var scope = factory.Services.CreateScope();
		//	using var context = scope.ServiceProvider.GetRequiredService<DataContext>();
		//	return await context.Inks.FirstAsync();
		//}

		//[Fact]
		//public async Task GetInkedUp() {
		//	await PrepareData();
		//	var pen = await GetFirst();
		//	var client = factory.CreateClient();
		//	var response = await client.GetAsync($"/api/InkedUps/{pen.Id}");
		//	response.EnsureSuccessStatusCode();

		//	var inks = await response.Content.ReadFromJsonAsync<FountainPenDownloadDTO>();
		//	Assert.NotNull(inks);
		//}

		//[Fact]
		//public async Task GetInkedUps() {
		//	await PrepareData();
		//	var client = factory.CreateClient();

		//	var response = await client.GetAsync("/api/InkedUps");
		//	response.EnsureSuccessStatusCode();

		//	var inks = await response.Content.ReadFromJsonAsync<IEnumerable<InkedUpDownloadDTO>>();
		//	Assert.NotNull(inks);
		//	Assert.NotEmpty(inks);
		//}

		//[Fact]
		//public async Task UpdateInkedUp() {
		//	await PrepareData();
		//	var client = factory.CreateClient();
		//	var ink = await GetFirst();
		//	var dto = ink.Adapt<InkedUpUploadDTO>();
		//	var content = JsonContent.Create(dto);
		//	var response = await client.PutAsync($"/api/InkedUps/{dto.Id}", content);
		//	response.EnsureSuccessStatusCode();

		//	var inks = await response.Content.ReadFromJsonAsync<InkedUpDownloadDTO>();
		//	Assert.NotNull(inks);
		//}

		//[Fact]
		//public async Task DeleteFountainPen() {
		//	await PrepareData();
		//	var pen = await GetFirst();
		//	var client = factory.CreateClient();
		//	var response = await client.DeleteAsync($"/api/InkedUps/{pen.Id}");
		//	response.EnsureSuccessStatusCode();
		//	Assert.True(true);
		//}

		//[Fact]
		//public async Task AddFountainPen() {
		//	await PrepareData();
		//	var client = factory.CreateClient();
		//	var dto = new InkedUpUploadDTO(
		//		Id: 0, Maker: "Maker3", InkedUpName: "Model3", Comment: "test", Photo: "", Color: "#085172",
		//		Rating: 1, Ml: 50, ImageObjectKey: "");
		//	var content = JsonContent.Create(dto);
		//	var response = await client.PostAsync($"/api/InkedUps", content);
		//	response.EnsureSuccessStatusCode();

		//	var inks = await response.Content.ReadFromJsonAsync<InkedUpDownloadDTO>();
		//	Assert.NotNull(inks);
		//}
	}
}