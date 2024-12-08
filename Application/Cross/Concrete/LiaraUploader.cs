using Amazon.S3;
using Amazon.S3.Model;
using Application.Cross.Interface;
using Common.ExceptionType.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Cross.Concrete;

public class LiaraUploader : ILiaraUploader
{
    private readonly ILogger<LiaraUploader> _logger;
    private readonly IConfiguration _configuration;
    private readonly AmazonS3Config _config;
    private readonly Amazon.Runtime.BasicAWSCredentials _credentials;
    private readonly string _bucketName;


    public LiaraUploader(IConfiguration configuration, ILogger<LiaraUploader> logger)
    {
        _configuration = configuration;


        string endpoint = _configuration.GetSection("Liara:Endpoint").Value;
        _bucketName = _configuration.GetSection("Liara:BucketName").Value;
        _config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true,
            SignatureVersion = "4"
        };
        _credentials = new Amazon.Runtime.BasicAWSCredentials(
            _configuration.GetValue<string>("Liara:AccessKey"),
            _configuration.GetValue<string>("Liara:SecretKey")
        );
        _logger = logger;
    }

    public async Task<string> Upload(string folder, IFormFile file, string? subFolder)
    {
        using var client = new AmazonS3Client(_credentials, _config);
        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string objectKey;

        if (!string.IsNullOrWhiteSpace(subFolder)) objectKey = $"{folder}/{subFolder}/{fileName}";
        else objectKey = $"{folder}/{fileName}";

        try
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream).ConfigureAwait(false);

            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                InputStream = memoryStream,
            };

            await client.PutObjectAsync(request);
            _logger.LogInformation($"File '{objectKey}' uploaded successfully");
        }

        catch (AmazonS3Exception exception)
        {
            _logger.LogError(exception, $"Error uploading file '{objectKey}");
            throw exception ?? throw new ErrorException();
        }

        return fileName;
    }

    public async Task<MemoryStream> Get(string folderName, string fileName, string? subFolder)
    {
        using var client = new AmazonS3Client(_credentials, _config);
        string objectKey;
        if (!string.IsNullOrWhiteSpace(subFolder)) objectKey = $"{folderName}/{subFolder}/{fileName}";
        else objectKey = $"{folderName}/{fileName}";

        try
        {
            using var response = await client.GetObjectAsync(_bucketName, objectKey);
            using var responseStream = response.ResponseStream;
            using var memoryStream = new MemoryStream();
            await responseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }
        catch (AmazonS3Exception exception)
        {
            _logger.LogError(exception, $"Error fetching file from Liara '{fileName}");
            throw exception ?? throw new ErrorException();
        }
    }

    public async Task Delete(string folder, string fileName, string? subFolder)
    {
        using var client = new AmazonS3Client(_credentials, _config);
        string objectKey;
        if (!string.IsNullOrWhiteSpace(subFolder)) objectKey = $"{folder}/{subFolder}/{fileName}";
        else objectKey = $"{folder}/{fileName}";
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };

            await client.DeleteObjectAsync(deleteRequest);
            _logger.LogInformation($"File '{fileName} deleted successfully'");
        }
        catch (AmazonS3Exception exception)
        {
            _logger.LogError(exception, $"Error deleting file: {fileName}");
        }
    }
}