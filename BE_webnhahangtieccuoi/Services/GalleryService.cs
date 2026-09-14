using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Gallery;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class GalleryService : IGalleryService
{
    private readonly AppDbContext _db;

    public GalleryService(AppDbContext db)
    {
        _db = db;
    }

    // =========================
    // PUBLIC
    // =========================
    public async Task<List<GalleryAlbumDto>> GetAlbumsAsync(
        int? serviceCategoryId)
    {
        var query = _db.GalleryAlbums
            .Include(a => a.Items)
            .Where(a => a.IsActive)
            .AsQueryable();

        if (serviceCategoryId.HasValue)
        {
            query = query.Where(
                a => a.ServiceCategoryId == serviceCategoryId.Value
            );
        }

        return await query
            .OrderBy(a => a.Id)
            .Select(a => new GalleryAlbumDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                CoverImageUrl = a.CoverImageUrl,

                Items = a.Items
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new GalleryItemDto
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        VideoUrl = i.VideoUrl,
                        Caption = i.Caption
                    })
                    .ToList()
            })
            .ToListAsync();
    }

    // =========================
    // ADMIN - GET ALBUMS
    // =========================
    public async Task<List<GalleryAlbum>> GetAdminAlbumsAsync()
    {
        return await _db.GalleryAlbums
            .Include(a => a.Items)
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    // =========================
    // ADMIN - CREATE ALBUM
    // =========================
    public async Task<GalleryAlbum> CreateAlbumAsync(
        CreateUpdateGalleryAlbumDto dto)
    {
        var entity = new GalleryAlbum
        {
            Name = dto.Name,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl,
            ServiceCategoryId = dto.ServiceCategoryId,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _db.GalleryAlbums.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    // =========================
    // ADMIN - ADD ITEM
    // =========================
    public async Task<GalleryItem> AddItemAsync(
        CreateUpdateGalleryItemDto dto)
    {
        var albumExists = await _db.GalleryAlbums
            .AnyAsync(a => a.Id == dto.GalleryAlbumId);

        if (!albumExists)
        {
            throw new KeyNotFoundException(
                "Gallery album không tồn tại."
            );
        }

        var entity = new GalleryItem
        {
            GalleryAlbumId = dto.GalleryAlbumId,
            ImageUrl = dto.ImageUrl,
            VideoUrl = dto.VideoUrl,
            Caption = dto.Caption,
            DisplayOrder = dto.DisplayOrder
        };

        _db.GalleryItems.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    // =========================
    // ADMIN - DELETE ITEM
    // =========================
    public async Task<bool> DeleteItemAsync(int id)
    {
        var entity = await _db.GalleryItems.FindAsync(id);

        if (entity == null)
        {
            return false;
        }

        _db.GalleryItems.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }

    // =========================
    // ADMIN - DELETE ALBUM
    // =========================
    public async Task<bool> DeleteAlbumAsync(int id)
    {
        var entity = await _db.GalleryAlbums
            .Include(a => a.Items)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (entity == null)
        {
            return false;
        }

        if (entity.Items.Any())
        {
            _db.GalleryItems.RemoveRange(entity.Items);
        }

        _db.GalleryAlbums.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }
}