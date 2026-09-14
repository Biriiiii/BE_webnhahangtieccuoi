using BE_webnhahangtieccuoi.DTOs.Gallery;
using BE_webnhahangtieccuoi.Services;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/gallery")]
public class GalleryController : ControllerBase
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    [AllowAnonymous]
    [HttpGet("albums")]
    public async Task<ActionResult<List<GalleryAlbumDto>>> GetAlbums(
        [FromQuery] int? serviceCategoryId)
    {
        var data = await _galleryService.GetAlbumsAsync(
            serviceCategoryId
        );

        return Ok(data);
    }
}