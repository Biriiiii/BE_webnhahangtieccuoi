using BE_webnhahangtieccuoi.DTOs.Services;
using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IServiceService
{
    // =========================
    // PUBLIC - SERVICE CATEGORY
    // =========================
    Task<List<ServiceCategoryDto>> GetActiveCategoriesAsync();

    // =========================
    // PUBLIC - SERVICES
    // =========================
    Task<List<ServiceDto>> GetActiveServicesAsync(
        int? categoryId);

    Task<ServiceDto?> GetActiveServiceBySlugAsync(
        string slug);

    // =========================
    // ADMIN - SERVICE CATEGORY
    // =========================
    Task<List<ServiceCategory>> GetAllCategoriesAsync();

    Task<ServiceCategory> CreateCategoryAsync(
        CreateUpdateServiceCategoryDto dto);

    Task<bool> UpdateCategoryAsync(
        int id,
        CreateUpdateServiceCategoryDto dto);

    Task<bool> DeleteCategoryAsync(
        int id);

    // =========================
    // ADMIN - SERVICES
    // =========================
    Task<List<Service>> GetAllServicesAsync();

    Task<Service?> GetServiceByIdAsync(
        int id);

    Task<Service> CreateServiceAsync(
        CreateUpdateServiceDto dto);

    Task<bool> UpdateServiceAsync(
        int id,
        CreateUpdateServiceDto dto);

    Task<bool> DeleteServiceAsync(
        int id);
}