using BE_webnhahangtieccuoi.DTOs.Gallery;
using BE_webnhahangtieccuoi.Services;
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

    public AdminGalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    // GET ALBUMS
    [HttpGet("albums")]
    public async Task<IActionResult> GetAlbums()
    {
        var data = await _galleryService.GetAdminAlbumsAsync();

        return Ok(data);
    }

    // CREATE ALBUM
    [HttpPost("albums")]
    public async Task<IActionResult> CreateAlbum(
        CreateUpdateGalleryAlbumDto dto)
    {
        var entity = await _galleryService.CreateAlbumAsync(dto);

        return Ok(entity);
    }

    // ADD ITEM
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(
        CreateUpdateGalleryItemDto dto)
    {
        try
        {
            var entity = await _galleryService.AddItemAsync(dto);

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

    // DELETE ITEM
    [HttpDelete("items/{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var result = await _galleryService.DeleteItemAsync(id);

        if (!result)
            return NotFound(new
            {
                message = "Gallery item không tồn tại."
            });

        return NoContent();
    }

    // DELETE ALBUM
    [HttpDelete("albums/{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var result = await _galleryService.DeleteAlbumAsync(id);

        if (!result)
            return NotFound(new
            {
                message = "Gallery album không tồn tại."
            });

        return NoContent();
    }
}