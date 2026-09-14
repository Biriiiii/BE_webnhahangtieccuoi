using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Menus;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class MenuService : IMenuService
{
    private readonly AppDbContext _db;

    public MenuService(AppDbContext db)
    {
        _db = db;
    }


    // =========================================================
    // PUBLIC - LẤY MENU
    // =========================================================

    public async Task<List<MenuGroupDto>> GetGroupsAsync(
        int? serviceCategoryId)
    {
        var query = _db.MenuGroups
            .Include(g => g.MenuItems)
            .Include(g => g.MenuPackages)
                .ThenInclude(p => p.MenuPackageItems)
                .ThenInclude(pi => pi.MenuItem)
            .Where(g => g.IsActive)
            .AsQueryable();

        if (serviceCategoryId.HasValue)
        {
            query = query.Where(
                g => g.ServiceCategoryId ==
                     serviceCategoryId.Value);
        }

        var groups = await query
            .OrderBy(g => g.Id)
            .ToListAsync();

        var result = groups.Select(g => new MenuGroupDto
        {
            Id = g.Id,

            Name = g.Name,

            Description = g.Description,

            MenuItems = g.MenuItems
                .Where(i => i.IsActive)
                .OrderBy(i => i.Id)
                .Select(i => new MenuItemDto
                {
                    Id = i.Id,

                    MenuGroupId =
                        i.MenuGroupId,

                    Name = i.Name,

                    Description =
                        i.Description,

                    ImageUrl =
                        i.ImageUrl,

                    UnitPrice =
                        i.UnitPrice
                })
                .ToList(),

            MenuPackages = g.MenuPackages
                .Where(p => p.IsActive)
                .OrderBy(p => p.Id)
                .Select(p => new MenuPackageDto
                {
                    Id = p.Id,

                    MenuGroupId =
                        p.MenuGroupId,

                    Name = p.Name,

                    Description =
                        p.Description,

                    PricePerTray =
                        p.PricePerTray,

                    IncludedItemNames =
                        p.MenuPackageItems
                            .Select(pi => pi.MenuItem.Name)
                            .ToList()
                })
                .ToList()

        }).ToList();

        return result;
    }


    // =========================================================
    // ADMIN - MENU GROUPS
    // =========================================================

    public async Task<List<MenuGroup>> GetAllGroupsAsync()
    {
        return await _db.MenuGroups
            .Include(g => g.MenuItems)
            .Include(g => g.MenuPackages)
            .OrderBy(g => g.Id)
            .ToListAsync();
    }


    public async Task<MenuGroup> CreateGroupAsync(
        CreateUpdateMenuGroupDto dto)
    {
        var entity = new MenuGroup
        {
            ServiceCategoryId =
                dto.ServiceCategoryId,

            Name =
                dto.Name,

            Description =
                dto.Description,

            IsActive =
                dto.IsActive
        };

        _db.MenuGroups.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }


    // =========================================================
    // ADMIN - MENU ITEMS
    // =========================================================

    public async Task<MenuItem> CreateItemAsync(
        CreateUpdateMenuItemDto dto)
    {
        var groupExists = await _db.MenuGroups
            .AnyAsync(g => g.Id == dto.MenuGroupId);

        if (!groupExists)
        {
            throw new KeyNotFoundException(
                "Nhóm menu không tồn tại.");
        }

        var entity = new MenuItem
        {
            MenuGroupId =
                dto.MenuGroupId,

            Name =
                dto.Name,

            Description =
                dto.Description,

            ImageUrl =
                dto.ImageUrl,

            UnitPrice =
                dto.UnitPrice,

            IsActive =
                dto.IsActive
        };

        _db.MenuItems.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }


    public async Task<bool> UpdateItemAsync(
        int id,
        CreateUpdateMenuItemDto dto)
    {
        var entity = await _db.MenuItems
            .FindAsync(id);

        if (entity == null)
            return false;

        var groupExists = await _db.MenuGroups
            .AnyAsync(g => g.Id == dto.MenuGroupId);

        if (!groupExists)
        {
            throw new KeyNotFoundException(
                "Nhóm menu không tồn tại.");
        }

        entity.MenuGroupId =
            dto.MenuGroupId;

        entity.Name =
            dto.Name;

        entity.Description =
            dto.Description;

        entity.ImageUrl =
            dto.ImageUrl;

        entity.UnitPrice =
            dto.UnitPrice;

        entity.IsActive =
            dto.IsActive;

        await _db.SaveChangesAsync();

        return true;
    }


    public async Task<bool> DeleteItemAsync(int id)
    {
        var entity = await _db.MenuItems
            .FindAsync(id);

        if (entity == null)
            return false;

        _db.MenuItems.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }


    // =========================================================
    // ADMIN - MENU PACKAGES
    // =========================================================

    public async Task<MenuPackage> CreatePackageAsync(
        CreateUpdateMenuPackageDto dto)
    {
        var groupExists = await _db.MenuGroups
            .AnyAsync(g => g.Id == dto.MenuGroupId);

        if (!groupExists)
        {
            throw new KeyNotFoundException(
                "Nhóm menu không tồn tại.");
        }

        var entity = new MenuPackage
        {
            MenuGroupId =
                dto.MenuGroupId,

            Name =
                dto.Name,

            Description =
                dto.Description,

            PricePerTray =
                dto.PricePerTray,

            IsActive =
                dto.IsActive
        };

        // =====================================================
        // KIỂM TRA CÁC MÓN TRONG GÓI
        // =====================================================

        if (dto.MenuItemIds != null &&
            dto.MenuItemIds.Any())
        {
            var existingItemIds =
                await _db.MenuItems
                    .Where(i =>
                        dto.MenuItemIds.Contains(i.Id))
                    .Select(i => i.Id)
                    .ToListAsync();

            foreach (var itemId in dto.MenuItemIds.Distinct())
            {
                if (!existingItemIds.Contains(itemId))
                    continue;

                entity.MenuPackageItems.Add(
                    new MenuPackageItem
                    {
                        MenuItemId = itemId
                    });
            }
        }

        _db.MenuPackages.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }


    public async Task<bool> DeletePackageAsync(int id)
    {
        var entity = await _db.MenuPackages
            .Include(p => p.MenuPackageItems)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
            return false;

        // Xóa các món liên kết trước
        if (entity.MenuPackageItems.Any())
        {
            _db.MenuPackageItems.RemoveRange(
                entity.MenuPackageItems);
        }

        _db.MenuPackages.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }
}