using BE_webnhahangtieccuoi.DTOs.Gallery;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/gallery")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminGalleryController : ControllerBase
{
    private readonly IGalleryService _galleryService;

    public AdminGalleryController(
        IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    // =========================
    // GET ALBUMS
    // =========================
    [HttpGet("albums")]
    public async Task<IActionResult> GetAlbums()
    {
        var data = await _galleryService.GetAdminAlbumsAsync();

        return Ok(data);
    }

    // =========================
    // CREATE ALBUM
    // =========================
    [HttpPost("albums")]
    public async Task<IActionResult> CreateAlbum(
        CreateUpdateGalleryAlbumDto dto)
    {
        var entity =
            await _galleryService.CreateAlbumAsync(dto);

        return Ok(entity);
    }

    // =========================
    // ADD 1 ITEM
    // =========================
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(
        CreateUpdateGalleryItemDto dto)
    {
        try
        {
            var entity =
                await _galleryService.AddItemAsync(dto);

            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    // =========================
    // UPLOAD NHIỀU ẢNH
    // =========================
    [HttpPost("albums/{albumId}/upload")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> UploadImages(
        int albumId,
        [FromForm] List<IFormFile> files,
        [FromForm] string? caption)
    {
        try
        {
            var items =
                await _galleryService.AddItemsAsync(
                    albumId,
                    files,
                    caption
                );

            return Ok(new
            {
                message = $"Upload thành công {items.Count} ảnh.",
                items
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = ex.Message
            });
        }
    }

    // =========================
    // DELETE ITEM
    // =========================
    [HttpDelete("items/{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var result =
            await _galleryService.DeleteItemAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Gallery item không tồn tại."
            });
        }

        return NoContent();
    }

    // =========================
    // DELETE ALBUM
    // =========================
    [HttpDelete("albums/{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var result =
            await _galleryService.DeleteAlbumAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Gallery album không tồn tại."
            });
        }

        return NoContent();
    }

    // =========================
    // SYNC GOOGLE DRIVE
    // =========================
    [HttpPost("albums/{id}/sync-drive")]
    public async Task<IActionResult> SyncDrive(int id)
    {
        try
        {
            var res = await _galleryService.SyncGoogleDriveFolderAsync(id);
            return Ok(new
            {
                count = res.AddedCount,
                total = res.TotalCount,
                message = res.AddedCount > 0 
                    ? $"Đã đồng bộ {res.AddedCount} ảnh mới từ Google Drive vào Album!"
                    : $"Album đã đồng bộ tất cả ảnh mới nhất từ Drive (Tổng {res.TotalCount} ảnh)."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Đồng bộ Google Drive thất bại: {ex.Message}" });
        }
    }
}