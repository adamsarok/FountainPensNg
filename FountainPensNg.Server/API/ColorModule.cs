using System;
using Carter;
using FountainPensNg.Server.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace FountainPensNg.Server.API;

public class ColorModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapGet("/api/Color/CieLchDistance", async ([FromQuery] string color) => {
            var cielab = ColorHelper.ToCIELAB(color);
			   var cieLch = ColorHelper.ToCieLch(cielab);
			   return Results.Ok(ColorHelper.GetEuclideanDistanceToReference(cieLch));
         })
            .WithTags("Color")
            .WithName("GetCieLchDistance");
	}
}
