using Amazon.Runtime;
using Amazon.S3;
using FountainPensNg.Server.API;
using FountainPensNg.Server.Config;
using FountainPensNg.Server.Data;
using ImageMagick;
using System;
using System.IO;
using System.Net.Http;
using static FountainPensNg.Server.API.R2Module;

namespace FountainPensNg.Server.Services;
public interface IR2UploadService {
	Task<Guid> UploadImage(IFormFile file);
}
public class R2UploadService(IPresignedUrlService presignedUrlService, ILogger<R2UploadService> logger, R2Configuration configuration, IHttpClientFactory httpClientFactory) 
	: IR2UploadService {
	public async Task<Guid> UploadImage(IFormFile file) {
		if (file == null || file.Length == 0) throw new BadRequestException("No file uploaded");
		if (string.IsNullOrWhiteSpace(configuration.AccountId)) throw new ServerException("R2 not configured");
		if (file.Length > configuration.MaxFileSizeKb * 1024) {
			throw new BadRequestException($"File size exceeds the maximum limit of {configuration.MaxFileSizeKb}kb");
		}
		using var stream = file.OpenReadStream();
		if (!IsValidImageFile(stream, logger)) throw new BadRequestException("Invalid image file");

		var guid = Guid.NewGuid();
		var presignedUrl = presignedUrlService.GetUrl(guid, HttpVerb.PUT);

		var httpClient = httpClientFactory.CreateClient();
		var response = await httpClient.PutAsync(presignedUrl, new StreamContent(stream));
		if (!response.IsSuccessStatusCode) {
			logger.LogError("Failed to upload image to R2: {StatusCode}", response.StatusCode);
			throw new ServerException("Failed to upload image");
		}
		return guid;
	}
	private bool IsValidImageFile(Stream stream, ILogger<R2UploadService> logger) {
		try {
			using var image = new MagickImage(stream);
			stream.Position = 0;
			return true;
		} catch (Exception ex) {
			logger.Log(LogLevel.Error, ex, "Upload is not a valid image file");
			return false;
		}
	}
}
