using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Banners;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class BannerService : IBannerService
{
    private readonly AppDbContext _db;

    public BannerService(AppDbContext db)
    {
        _db = db;
    }

    // =========================
    // PUBLIC - GET BANNERS
    // =========================
    public async Task<List<BannerDto>> GetActiveBannersAsync()
    {
        return await _db.Banners
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .Select(b => new BannerDto
            {
                Id = b.Id,
                ImageUrl = b.ImageUrl,
                Title = b.Title,
                LinkUrl = b.LinkUrl
            })
            .ToListAsync();
    }

    // =========================
    // ADMIN - GET ALL
    // =========================
    public async Task<List<Banner>> GetAllBannersAsync()
    {
        return await _db.Banners
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();
    }

    // =========================
    // ADMIN - CREATE
    // =========================
    public async Task<Banner> CreateBannerAsync(
        CreateUpdateBannerDto dto)
    {
        var entity = new Banner
        {
            ImageUrl = dto.ImageUrl,
            Title = dto.Title,
            LinkUrl = dto.LinkUrl,
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        _db.Banners.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    // =========================
    // ADMIN - DELETE
    // =========================
    public async Task<bool> DeleteBannerAsync(int id)
    {
        var entity = await _db.Banners.FindAsync(id);

        if (entity == null)
        {
            return false;
        }

        _db.Banners.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }
}