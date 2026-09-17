using BE_webnhahangtieccuoi.DTOs.Gallery;
using BE_webnhahangtieccuoi.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IGalleryService
{
    Task<List<GalleryAlbumDto>> GetAlbumsAsync(int? serviceCategoryId);

    Task<List<GalleryAlbum>> GetAdminAlbumsAsync();

    Task<GalleryAlbum> CreateAlbumAsync(
        CreateUpdateGalleryAlbumDto dto
    );

    Task<GalleryItem> AddItemAsync(
        CreateUpdateGalleryItemDto dto
    );

    // Upload nhiều ảnh
    Task<List<GalleryItem>> AddItemsAsync(
        int galleryAlbumId,
        List<IFormFile> files,
        string? caption = null
    );

    Task<bool> DeleteItemAsync(int id);

    Task<bool> DeleteAlbumAsync(int id);

    Task<(int AddedCount, int TotalCount)> SyncGoogleDriveFolderAsync(int galleryAlbumId);
}