using Microsoft.AspNetCore.Http;

namespace Application.Cross.Interface;

public interface IUploader
{
   Task<string> UploadFile(IFormFile file, string path, string exteraPath, string? userId = null);
}