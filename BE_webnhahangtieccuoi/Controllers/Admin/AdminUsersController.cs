using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

// Chỉ SuperAdmin được quản lý tài khoản nhân viên
[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "SuperAdmin")]
public class AdminUsersController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminUsersController(
        IUserService userService)
    {
        _userService = userService;
    }


    // =========================================================
    // GET ALL USERS
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data =
            await _userService.GetAllAsync();

        return Ok(data);
    }


    // =========================================================
    // CREATE USER
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAdminUserDto dto)
    {
        try
        {
            var result =
                await _userService.CreateAsync(dto);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // =========================================================
    // DEACTIVATE USER
    // =========================================================

    [HttpPut("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var result =
            await _userService.DeactivateAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Tài khoản không tồn tại."
            });
        }

        return NoContent();
    }
}