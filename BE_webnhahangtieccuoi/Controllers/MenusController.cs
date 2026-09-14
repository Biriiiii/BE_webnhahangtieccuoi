using BE_webnhahangtieccuoi.DTOs.Menus;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/menus")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }


    // =========================================================
    // PUBLIC - LẤY MENU
    // =========================================================

    [AllowAnonymous]
    [HttpGet("groups")]
    public async Task<ActionResult<List<MenuGroupDto>>> GetGroups(
        [FromQuery] int? serviceCategoryId)
    {
        var result =
            await _menuService.GetGroupsAsync(
                serviceCategoryId);

        return Ok(result);
    }
}