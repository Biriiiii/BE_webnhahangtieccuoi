using BE_webnhahangtieccuoi.DTOs.Banners;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/banners")]
public class BannersController : ControllerBase
{
    private readonly IBannerService _bannerService;

    public BannersController(
        IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    // GET: api/banners
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<BannerDto>>> GetAll()
    {
        var data = await _bannerService.GetActiveBannersAsync();

        return Ok(data);
    }
}