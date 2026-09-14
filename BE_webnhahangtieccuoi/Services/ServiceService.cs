using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Services;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class ServiceService : IServiceService
{
    private readonly AppDbContext _db;

    public ServiceService(AppDbContext db)
    {
        _db = db;
    }

    // =========================================================
    // PUBLIC - SERVICE CATEGORY
    // =========================================================

    public async Task<List<ServiceCategoryDto>>
        GetActiveCategoriesAsync()
    {
        return await _db.ServiceCategories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new ServiceCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                ThumbnailUrl = c.ThumbnailUrl,
                DisplayOrder = c.DisplayOrder
            })
            .ToListAsync();
    }


    // =========================================================
    // PUBLIC - SERVICES
    // =========================================================

    public async Task<List<ServiceDto>>
        GetActiveServicesAsync(int? categoryId)
    {
        var query = _db.Services
            .Include(s => s.ServiceCategory)
            .Where(s => s.IsActive)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(
                s => s.ServiceCategoryId == categoryId.Value);
        }

        return await query
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new ServiceDto
            {
                Id = s.Id,
                ServiceCategoryId = s.ServiceCategoryId,

                ServiceCategoryName =
                    s.ServiceCategory.Name,

                Name = s.Name,
                Slug = s.Slug,
                ShortDescription = s.ShortDescription,
                Content = s.Content,
                ThumbnailUrl = s.ThumbnailUrl,
                BasePrice = s.BasePrice
            })
            .ToListAsync();
    }


    public async Task<ServiceDto?>
        GetActiveServiceBySlugAsync(string slug)
    {
        return await _db.Services
            .Include(s => s.ServiceCategory)
            .Where(s =>
                s.Slug == slug &&
                s.IsActive)
            .Select(s => new ServiceDto
            {
                Id = s.Id,
                ServiceCategoryId = s.ServiceCategoryId,

                ServiceCategoryName =
                    s.ServiceCategory.Name,

                Name = s.Name,
                Slug = s.Slug,
                ShortDescription = s.ShortDescription,
                Content = s.Content,
                ThumbnailUrl = s.ThumbnailUrl,
                BasePrice = s.BasePrice
            })
            .FirstOrDefaultAsync();
    }


    // =========================================================
    // ADMIN - SERVICE CATEGORY
    // =========================================================

    public async Task<List<ServiceCategory>>
        GetAllCategoriesAsync()
    {
        return await _db.ServiceCategories
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();
    }


    public async Task<ServiceCategory>
        CreateCategoryAsync(
            CreateUpdateServiceCategoryDto dto)
    {
        var entity = new ServiceCategory
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
            ThumbnailUrl = dto.ThumbnailUrl,
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        _db.ServiceCategories.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }


    public async Task<bool>
        UpdateCategoryAsync(
            int id,
            CreateUpdateServiceCategoryDto dto)
    {
        var entity =
            await _db.ServiceCategories.FindAsync(id);

        if (entity == null)
            return false;

        entity.Name = dto.Name;
        entity.Slug = dto.Slug;
        entity.Description = dto.Description;
        entity.ThumbnailUrl = dto.ThumbnailUrl;
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();

        return true;
    }


    public async Task<bool>
        DeleteCategoryAsync(int id)
    {
        var entity =
            await _db.ServiceCategories.FindAsync(id);

        if (entity == null)
            return false;

        _db.ServiceCategories.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }


    // =========================================================
    // ADMIN - SERVICES
    // =========================================================

    public async Task<List<Service>>
        GetAllServicesAsync()
    {
        return await _db.Services
            .Include(s => s.ServiceCategory)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync();
    }


    public async Task<Service?>
        GetServiceByIdAsync(int id)
    {
        return await _db.Services
            .Include(s => s.ServiceCategory)
            .FirstOrDefaultAsync(s => s.Id == id);
    }


    public async Task<Service>
        CreateServiceAsync(
            CreateUpdateServiceDto dto)
    {
        var categoryExists =
            await _db.ServiceCategories
                .AnyAsync(c => c.Id == dto.ServiceCategoryId);

        if (!categoryExists)
        {
            throw new KeyNotFoundException(
                "Danh mục dịch vụ không tồn tại.");
        }

        var entity = new Service
        {
            ServiceCategoryId = dto.ServiceCategoryId,
            Name = dto.Name,
            Slug = dto.Slug,
            ShortDescription = dto.ShortDescription,
            Content = dto.Content,
            ThumbnailUrl = dto.ThumbnailUrl,
            BasePrice = dto.BasePrice,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        _db.Services.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }


    public async Task<bool>
        UpdateServiceAsync(
            int id,
            CreateUpdateServiceDto dto)
    {
        var entity =
            await _db.Services.FindAsync(id);

        if (entity == null)
            return false;

        var categoryExists =
            await _db.ServiceCategories
                .AnyAsync(c => c.Id == dto.ServiceCategoryId);

        if (!categoryExists)
        {
            throw new KeyNotFoundException(
                "Danh mục dịch vụ không tồn tại.");
        }

        entity.ServiceCategoryId = dto.ServiceCategoryId;
        entity.Name = dto.Name;
        entity.Slug = dto.Slug;
        entity.ShortDescription = dto.ShortDescription;
        entity.Content = dto.Content;
        entity.ThumbnailUrl = dto.ThumbnailUrl;
        entity.BasePrice = dto.BasePrice;
        entity.IsActive = dto.IsActive;
        entity.DisplayOrder = dto.DisplayOrder;

        await _db.SaveChangesAsync();

        return true;
    }


    public async Task<bool>
        DeleteServiceAsync(int id)
    {
        var entity =
            await _db.Services.FindAsync(id);

        if (entity == null)
            return false;

        _db.Services.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }
}