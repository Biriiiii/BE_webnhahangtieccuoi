using BE_webnhahangtieccuoi.DTOs.News;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/news")]
public class NewsController : ControllerBase
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }


    // =========================================================
    // PUBLIC - DANH SÁCH BÀI VIẾT
    // =========================================================

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<NewsDto>>> GetAll()
    {
        var data =
            await _newsService.GetPublishedNewsAsync();

        return Ok(data);
    }


    // =========================================================
    // PUBLIC - CHI TIẾT THEO SLUG
    // =========================================================

    [AllowAnonymous]
    [HttpGet("{slug}")]
    public async Task<ActionResult<NewsDto>> GetBySlug(
        string slug)
    {
        var data =
            await _newsService
                .GetPublishedNewsBySlugAsync(slug);

        if (data == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy bài viết."
            });
        }

        return Ok(data);
    }
}