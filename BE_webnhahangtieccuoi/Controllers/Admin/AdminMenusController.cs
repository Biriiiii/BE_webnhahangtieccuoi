using BE_webnhahangtieccuoi.DTOs.Menus;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/menus")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminMenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public AdminMenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }


    // =========================================================
    // MENU GROUPS
    // =========================================================

    [HttpGet("groups")]
    public async Task<ActionResult<List<MenuGroup>>> GetGroups()
    {
        var data =
            await _menuService.GetAllGroupsAsync();

        return Ok(data);
    }


    [HttpPost("groups")]
    public async Task<ActionResult<MenuGroup>> CreateGroup(
        CreateUpdateMenuGroupDto dto)
    {
        try
        {
            var entity =
                await _menuService.CreateGroupAsync(dto);

            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    // =========================================================
    // MENU ITEMS
    // =========================================================

    [HttpPost("items")]
    public async Task<ActionResult<MenuItem>> CreateItem(
        CreateUpdateMenuItemDto dto)
    {
        try
        {
            var entity =
                await _menuService.CreateItemAsync(dto);

            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpPut("items/{id}")]
    public async Task<IActionResult> UpdateItem(
        int id,
        CreateUpdateMenuItemDto dto)
    {
        try
        {
            var result =
                await _menuService.UpdateItemAsync(
                    id,
                    dto);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Món ăn không tồn tại."
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


    [HttpDelete("items/{id}")]
    public async Task<IActionResult> DeleteItem(
        int id)
    {
        var result =
            await _menuService.DeleteItemAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Món ăn không tồn tại."
            });
        }

        return NoContent();
    }


    // =========================================================
    // MENU PACKAGES
    // =========================================================

    [HttpPost("packages")]
    public async Task<ActionResult<MenuPackage>> CreatePackage(
        CreateUpdateMenuPackageDto dto)
    {
        try
        {
            var entity =
                await _menuService.CreatePackageAsync(dto);

            return Ok(entity);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }


    [HttpDelete("packages/{id}")]
    public async Task<IActionResult> DeletePackage(
        int id)
    {
        var result =
            await _menuService.DeletePackageAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Gói mâm không tồn tại."
            });
        }

        return NoContent();
    }
}