using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace FountainPensNg.Server.Services {
	public class FileService : IFileService {
		private IWebHostEnvironment env { get; }
		private ILogger<FileService> logger { get; }
		private string servePath;
		private string uploadPath;

        public FileService(IWebHostEnvironment env, ILogger<FileService> logger) {
			this.env = env;
			this.logger = logger;
			this.servePath = env.IsProduction() ?
				"images" :
				Path.Combine(env.ContentRootPath, "wwwroot", "images");
			this.uploadPath = Path.Combine(env.ContentRootPath, "wwwroot", "images");
        }
		public async Task<string> UploadFile(IBrowserFile browserFile) {
			try {
                logger.LogWarning(browserFile.Name);
                var trustedFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(browserFile.Name);
				var path = Path.Combine(uploadPath,
					trustedFileName);
                logger.LogWarning(path);
                await using FileStream fs = new(path, FileMode.Create);
				await browserFile.OpenReadStream().CopyToAsync(fs);
				return string.IsNullOrWhiteSpace(trustedFileName) ? "" : trustedFileName;
				//todo:  "unsafe_uploads", safe upload with virus checking etc: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-8.0#security-considerations
			} catch (Exception ex) {
				logger.LogError("File: {Filename} Error: {Error}", browserFile.Name, ex.Message);
			}
			return "";
		}

		public void DeleteFile(string fileName) {
			throw new NotImplementedException();
		}

        public string GetImagePath(string fileName) {
			if (string.IsNullOrWhiteSpace(servePath) || string.IsNullOrWhiteSpace(fileName)) return "";
            return Path.Combine(servePath, fileName);
        }

    }
}
