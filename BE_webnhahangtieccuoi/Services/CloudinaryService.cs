using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BE_webnhahangtieccuoi.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration config)
    {
        var section = config.GetSection("CloudinarySettings");

        var cloudName = section["CloudName"];
        var apiKey = section["ApiKey"];
        var apiSecret = section["ApiSecret"];

        if (string.IsNullOrWhiteSpace(cloudName) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiSecret))
        {
            throw new InvalidOperationException(
                "Cloudinary chưa được cấu hình đầy đủ trong appsettings.json."
            );
        }

        var account = new Account(
            cloudName,
            apiKey,
            apiSecret
        );

        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadImageAsync(
        IFormFile file,
        string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException(
                "File ảnh không hợp lệ."
            );
        }

        using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(
                file.FileName,
                stream
            ),

            Folder = string.IsNullOrWhiteSpace(folder)
                ? "wedding-center"
                : folder
        };

        var uploadResult =
            await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new Exception(
                $"Cloudinary upload error: {uploadResult.Error.Message}"
            );
        }

        return uploadResult.SecureUrl.ToString();
    }
}