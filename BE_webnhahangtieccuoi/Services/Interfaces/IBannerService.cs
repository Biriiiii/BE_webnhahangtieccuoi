using BE_webnhahangtieccuoi.DTOs.Banners;
using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IBannerService
{
    Task<List<BannerDto>> GetActiveBannersAsync();

    Task<List<Banner>> GetAllBannersAsync();

    Task<Banner> CreateBannerAsync(
        CreateUpdateBannerDto dto
    );

    Task<bool> DeleteBannerAsync(int id);
}