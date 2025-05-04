using FountainPensNg.Server.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FountainPensNg.xTests
{
    public class ColorTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> 
    {
		[Fact]
		public async Task GetCielchDistance() {
			var client = factory.CreateClient();
			var response = await client.GetAsync($"/api/colors/cie-lch-distance?color=%23FFFFFF");
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadFromJsonAsync<float>();
			Assert.True(result > 300);
		}
	}
}
