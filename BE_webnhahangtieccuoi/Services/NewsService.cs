using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.News;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class NewsService : INewsService
{
    private readonly AppDbContext _db;

    public NewsService(AppDbContext db)
    {
        _db = db;
    }


    // =========================================================
    // PUBLIC - LẤY DANH SÁCH BÀI VIẾT
    // =========================================================

    public async Task<List<NewsDto>> GetPublishedNewsAsync()
    {
        return await _db.News
            .Where(n => n.IsPublished)
            .OrderByDescending(n => n.PublishedAt)
            .Select(n => new NewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Slug = n.Slug,
                ThumbnailUrl = n.ThumbnailUrl,
                Summary = n.Summary,
                Content = n.Content,
                PublishedAt = n.PublishedAt
            })
            .ToListAsync();
    }


    // =========================================================
    // PUBLIC - CHI TIẾT BÀI VIẾT THEO SLUG
    // =========================================================

    public async Task<NewsDto?> GetPublishedNewsBySlugAsync(
        string slug)
    {
        return await _db.News
            .Where(n =>
                n.Slug == slug &&
                n.IsPublished)
            .Select(n => new NewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Slug = n.Slug,
                ThumbnailUrl = n.ThumbnailUrl,
                Summary = n.Summary,
                Content = n.Content,
                PublishedAt = n.PublishedAt
            })
            .FirstOrDefaultAsync();
    }


    // =========================================================
    // ADMIN - LẤY TẤT CẢ BÀI VIẾT
    // =========================================================

    public async Task<List<News>> GetAllNewsAsync()
    {
        return await _db.News
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }


    // =========================================================
    // ADMIN - TẠO BÀI VIẾT
    // =========================================================

    public async Task<News> CreateNewsAsync(
        CreateUpdateNewsDto dto)
    {
        var now = DateTime.UtcNow;

        var entity = new News
        {
            Title = dto.Title,

            Slug = dto.Slug,

            ThumbnailUrl = dto.ThumbnailUrl,

            Summary = dto.Summary,

            Content = dto.Content,

            MetaTitle = dto.MetaTitle,

            MetaDescription = dto.MetaDescription,

            IsPublished = dto.IsPublished,

            PublishedAt = now,

            CreatedAt = now
        };

        _db.News.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }


    // =========================================================
    // ADMIN - CẬP NHẬT BÀI VIẾT
    // =========================================================

    public async Task<bool> UpdateNewsAsync(
        int id,
        CreateUpdateNewsDto dto)
    {
        var entity = await _db.News
            .FindAsync(id);

        if (entity == null)
            return false;

        entity.Title = dto.Title;

        entity.Slug = dto.Slug;

        entity.ThumbnailUrl =
            dto.ThumbnailUrl;

        entity.Summary =
            dto.Summary;

        entity.Content =
            dto.Content;

        entity.MetaTitle =
            dto.MetaTitle;

        entity.MetaDescription =
            dto.MetaDescription;

        // Nếu chuyển từ chưa xuất bản -> xuất bản
        if (!entity.IsPublished &&
            dto.IsPublished)
        {
            entity.PublishedAt =
                DateTime.UtcNow;
        }

        // Nếu đang xuất bản -> bỏ xuất bản
        if (!entity.IsPublished && dto.IsPublished)
        {
            entity.PublishedAt = DateTime.UtcNow;
        }

        entity.IsPublished = dto.IsPublished;

        entity.IsPublished =
            dto.IsPublished;

        await _db.SaveChangesAsync();

        return true;
    }


    // =========================================================
    // ADMIN - XÓA BÀI VIẾT
    // =========================================================

    public async Task<bool> DeleteNewsAsync(int id)
    {
        var entity = await _db.News
            .FindAsync(id);

        if (entity == null)
            return false;

        _db.News.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }
}