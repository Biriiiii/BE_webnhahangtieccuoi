using BE_webnhahangtieccuoi.DTOs.Services;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/services")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminServicesController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public AdminServicesController(
        IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Service>>> GetAll()
    {
        var data =
            await _serviceService.GetAllServicesAsync();

        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> GetById(int id)
    {
        var data =
            await _serviceService.GetServiceByIdAsync(id);

        if (data == null)
        {
            return NotFound(new
            {
                message = "Dịch vụ không tồn tại."
            });
        }

        return Ok(data);
    }

    [HttpPost]
    public async Task<ActionResult<Service>> Create(
        CreateUpdateServiceDto dto)
    {
        try
        {
            var entity =
                await _serviceService.CreateServiceAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.Id },
                entity);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        CreateUpdateServiceDto dto)
    {
        try
        {
            var result =
                await _serviceService.UpdateServiceAsync(
                    id,
                    dto);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Dịch vụ không tồn tại."
                });
            }

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _serviceService.DeleteServiceAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Dịch vụ không tồn tại."
            });
        }

        return NoContent();
    }
}