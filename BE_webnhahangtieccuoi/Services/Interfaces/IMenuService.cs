using BE_webnhahangtieccuoi.DTOs.Menus;
using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IMenuService
{
    // Public
    Task<List<MenuGroupDto>> GetGroupsAsync(int? serviceCategoryId);

    // Admin - Menu Groups
    Task<List<MenuGroup>> GetAllGroupsAsync();
    Task<MenuGroup> CreateGroupAsync(CreateUpdateMenuGroupDto dto);

    // Admin - Menu Items
    Task<MenuItem> CreateItemAsync(CreateUpdateMenuItemDto dto);
    Task<bool> UpdateItemAsync(int id, CreateUpdateMenuItemDto dto);
    Task<bool> DeleteItemAsync(int id);

    // Admin - Menu Packages
    Task<MenuPackage> CreatePackageAsync(CreateUpdateMenuPackageDto dto);
    Task<bool> DeletePackageAsync(int id);
}