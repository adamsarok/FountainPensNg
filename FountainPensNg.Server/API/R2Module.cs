
using Amazon.S3;
using FountainPensNg.Server.Config;
using FountainPensNg.Server.Data;
using FountainPensNg.Server.Services;

namespace FountainPensNg.Server.API;

public class R2Module : ICarterModule {
	public void AddRoutes(IEndpointRouteBuilder app) {
		app.MapGet("/api/images/get-presigned-url/{id}", async (Guid id, IPresignedUrlService presignedUrlService) => {
			return presignedUrlService.GetUrl(id, HttpVerb.GET);
		})
			.WithTags("Images")
			.WithName("GetImageUrlDownload");
	}
}
