using BE_webnhahangtieccuoi.DTOs.Banners;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/banners")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminBannersController : ControllerBase
{
    private readonly IBannerService _bannerService;

    public AdminBannersController(
        IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    // GET: api/admin/banners
    [HttpGet]
    public async Task<ActionResult<List<Banner>>> GetAll()
    {
        var data = await _bannerService.GetAllBannersAsync();

        return Ok(data);
    }

    // POST: api/admin/banners
    [HttpPost]
    public async Task<ActionResult<Banner>> Create(
        CreateUpdateBannerDto dto)
    {
        var entity = await _bannerService.CreateBannerAsync(dto);

        return Ok(entity);
    }

    // DELETE: api/admin/banners/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bannerService.DeleteBannerAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Banner không tồn tại."
            });
        }

        return NoContent();
    }
}