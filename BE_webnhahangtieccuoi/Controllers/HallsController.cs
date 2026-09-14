using BE_webnhahangtieccuoi.DTOs.Halls;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/halls")]
public class HallsController : ControllerBase
{
    private readonly IHallService _hallService;

    public HallsController(IHallService hallService)
    {
        _hallService = hallService;
    }

    // GET: api/halls
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<HallDto>>> GetAll()
    {
        var data = await _hallService.GetActiveHallsAsync();

        return Ok(data);
    }

    // GET: api/halls/{id}
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<HallDto>> GetById(int id)
    {
        var data = await _hallService.GetActiveHallByIdAsync(id);

        if (data == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sảnh."
            });
        }

        return Ok(data);
    }
}