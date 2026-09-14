using BE_webnhahangtieccuoi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/upload")]
[Authorize(Roles = "SuperAdmin,Admin,Staff")]
public class UploadController : ControllerBase
{
    private readonly ICloudinaryService _cloudinaryService;

    public UploadController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    /// <summary>
    /// Upload file ảnh lên Cloudinary và nhận lại URL trực tiếp.
    /// Các folder gợi ý:
    ///  - wedding-center/banners
    ///  - wedding-center/gallery
    ///  - wedding-center/services
    ///  - wedding-center/menu-items
    ///  - wedding-center/halls
    ///  - wedding-center/news
    ///  - wedding-center/testimonials
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string? folder)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "Vui lòng chọn file ảnh để tải lên." });
        }

        var targetFolder = string.IsNullOrWhiteSpace(folder) ? "wedding-center" : folder;
        try
        {
            var imageUrl = await _cloudinaryService.UploadImageAsync(file, targetFolder);
            return Ok(new { url = imageUrl, message = "Upload ảnh lên Cloudinary thành công." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

