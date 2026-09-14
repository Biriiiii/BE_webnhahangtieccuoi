using Microsoft.AspNetCore.Http;

namespace BE_webnhahangtieccuoi.Services;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, string folder);
}

