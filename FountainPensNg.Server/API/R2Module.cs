
using Amazon.S3;
using FountainPensNg.Server.Config;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Services;

namespace FountainPensNg.Server.API;

public class R2Module : ICarterModule {
	public void AddRoutes(IEndpointRouteBuilder app) {
		app.MapGet("/api/images/{id}", async (Guid id, IPresignedUrlService presignedUrlService) => {
			return presignedUrlService.GetUrl(id, HttpVerb.GET);
		})
			.WithTags("Images")
			.WithName("GetImageUrlDownload");
		app.MapPut("/api/images", async (IFormFile file,
			IR2UploadService r2UploadService) => {
				var result = await r2UploadService.UploadImage(file);
				return Results.Ok(new UploadResponse(result));
			})
			.WithTags("Images")
			.WithName("UploadImage")
			.DisableAntiforgery();
	}
}
