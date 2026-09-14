using BE_webnhahangtieccuoi.DTOs.Services;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/service-categories")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminServiceCategoriesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public AdminServiceCategoriesController(
        IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceCategory>>> GetAll()
    {
        var data = await _serviceService.GetAllCategoriesAsync();

        return Ok(data);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceCategory>> Create(
        CreateUpdateServiceCategoryDto dto)
    {
        var entity = await _serviceService.CreateCategoryAsync(dto);

        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        CreateUpdateServiceCategoryDto dto)
    {
        var result = await _serviceService.UpdateCategoryAsync(
            id,
            dto);

        if (!result)
        {
            return NotFound(new
            {
                message = "Danh mục dịch vụ không tồn tại."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _serviceService.DeleteCategoryAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Danh mục dịch vụ không tồn tại."
            });
        }

        return NoContent();
    }
}