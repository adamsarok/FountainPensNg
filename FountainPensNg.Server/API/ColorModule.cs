using System;
using Carter;
using FountainPensNg.Server.Helpers;

namespace FountainPensNg.Server.API;

public class ColorModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapGet("/api/Colors/CieLch", async (string colorHex) => {
            var cielab = ColorHelper.ToCIELAB(colorHex);
            return ColorHelper.ToCieLch(cielab);
         })
            .WithTags("Colors")
            .WithName("GetCieLch");

    }
}
