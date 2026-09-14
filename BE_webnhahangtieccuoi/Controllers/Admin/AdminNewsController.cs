using BE_webnhahangtieccuoi.DTOs.News;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/news")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminNewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public AdminNewsController(INewsService newsService)
    {
        _newsService = newsService;
    }


    // =========================================================
    // GET ALL
    // =========================================================

    [HttpGet]
    public async Task<ActionResult<List<News>>> GetAll()
    {
        var data =
            await _newsService.GetAllNewsAsync();

        return Ok(data);
    }


    // =========================================================
    // CREATE
    // =========================================================

    [HttpPost]
    public async Task<ActionResult<News>> Create(
        CreateUpdateNewsDto dto)
    {
        var entity =
            await _newsService.CreateNewsAsync(dto);

        return Ok(entity);
    }


    // =========================================================
    // UPDATE
    // =========================================================

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        CreateUpdateNewsDto dto)
    {
        var result =
            await _newsService.UpdateNewsAsync(
                id,
                dto);

        if (!result)
        {
            return NotFound(new
            {
                message = "Bài viết không tồn tại."
            });
        }

        return NoContent();
    }


    // =========================================================
    // DELETE
    // =========================================================

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        var result =
            await _newsService.DeleteNewsAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Bài viết không tồn tại."
            });
        }

        return NoContent();
    }
}