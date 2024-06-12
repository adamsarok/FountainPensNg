using Microsoft.AspNetCore.Components.Forms;

namespace FountainPensNg.Server.Services {
	public interface IFileService {
		Task<string> UploadFile(IBrowserFile browserFile);
		void DeleteFile (string fileName);
		string GetImagePath(string fileName);
	}
}
