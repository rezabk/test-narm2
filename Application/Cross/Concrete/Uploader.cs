using Application.Cross.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace Application.Cross.Concrete;

public class Uploader : IUploader
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public Uploader(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _configuration = configuration;
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string> UploadFile(IFormFile file, string path, string extraPath, string? userId = null)
    {
        var fileFullPath = "!bad path!";
        try
        {
            //var firstpath = path + _configuration.GetSection("File:SavePath").Value;
            var extension = Path.GetExtension(file.FileName);
            var fileName = "";
            if (string.IsNullOrEmpty(userId))
                fileName = Guid.NewGuid() + extension;
            else
                fileName = userId + "_" + Guid.NewGuid() + extension;
            var tempPath = Path.Combine(path, extraPath);
            fileFullPath = Path.Combine(tempPath, fileName);
            if (!Directory.Exists(Path.Combine(path, extraPath)))
                Directory.CreateDirectory(Path.Combine(path, extraPath));
            switch (extension)
            {
                case ".jpg":
                    var imageResized = await ResizeImage(file);
                    imageResized.Save(fileFullPath);
                    break;

                default:
                    using (var stream = File.Create(fileFullPath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    ;
                    break;
            }

            return fileName;
        }
        catch (Exception)
        {
            throw new Exception($"File Upload not working Correctly in path : {fileFullPath} ");
        }
    }

    private Task<Image> ResizeImage(IFormFile file)
    {
        var image = Image.Load(file.OpenReadStream());
        var imageH = image.Height;
        var imageW = image.Width;
        var maxH = int.Parse(_configuration.GetSection("File:MaxWidth").Value);
        var maxW = int.Parse(_configuration.GetSection("File:MaxHeight").Value);
        while (imageH > maxH && imageW > maxW)
        {
            imageH = imageH * 3 / 4;
            imageW = imageW * 3 / 4;
        }

        image.Mutate(x => x.Resize(imageW, imageH));
        return Task.FromResult(image);
    }
}