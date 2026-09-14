using BE_webnhahangtieccuoi.DTOs.Services;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesController(
        IServiceService serviceService)
    {
        _serviceService = serviceService;
    }


    // =========================================================
    // GET CATEGORIES
    // =========================================================

    [AllowAnonymous]
    [HttpGet("categories")]
    public async Task<ActionResult<List<ServiceCategoryDto>>>
        GetCategories()
    {
        var data =
            await _serviceService.GetActiveCategoriesAsync();

        return Ok(data);
    }


    // =========================================================
    // GET SERVICES
    // =========================================================

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ServiceDto>>>
        GetServices(
            [FromQuery] int? categoryId)
    {
        var data =
            await _serviceService.GetActiveServicesAsync(
                categoryId);

        return Ok(data);
    }


    // =========================================================
    // GET SERVICE BY SLUG
    // =========================================================

    [AllowAnonymous]
    [HttpGet("{slug}")]
    public async Task<ActionResult<ServiceDto>>
        GetBySlug(string slug)
    {
        var data =
            await _serviceService
                .GetActiveServiceBySlugAsync(slug);

        if (data == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy dịch vụ."
            });
        }

        return Ok(data);
    }
}