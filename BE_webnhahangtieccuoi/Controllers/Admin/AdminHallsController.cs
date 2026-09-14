using BE_webnhahangtieccuoi.DTOs.Halls;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/halls")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminHallsController : ControllerBase
{
    private readonly IHallService _hallService;

    public AdminHallsController(IHallService hallService)
    {
        _hallService = hallService;
    }

    // GET: api/admin/halls
    [HttpGet]
    public async Task<ActionResult<List<Hall>>> GetAll()
    {
        var data = await _hallService.GetAllHallsAsync();

        return Ok(data);
    }

    // POST: api/admin/halls
    [HttpPost]
    public async Task<ActionResult<Hall>> Create(
        CreateUpdateHallDto dto)
    {
        var entity = await _hallService.CreateHallAsync(dto);

        return Ok(entity);
    }

    // PUT: api/admin/halls/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        CreateUpdateHallDto dto)
    {
        var result = await _hallService.UpdateHallAsync(id, dto);

        if (!result)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sảnh."
            });
        }

        return NoContent();
    }

    // DELETE: api/admin/halls/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _hallService.DeleteHallAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sảnh."
            });
        }

        return NoContent();
    }
}