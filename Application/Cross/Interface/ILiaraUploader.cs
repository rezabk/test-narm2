using Microsoft.AspNetCore.Http;

namespace Application.Cross.Interface;

public interface ILiaraUploader
{
    Task<string> Upload(string folder,IFormFile file,string? subFolder);

     Task<MemoryStream> Get(string folderName,string fileName,string? subFolder);

    Task Delete(string folder, string fileName,string? subFolder);
}