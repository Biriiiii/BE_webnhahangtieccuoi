using BE_webnhahangtieccuoi.DTOs.Gallery;
using BE_webnhahangtieccuoi.Models.Entities;

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

    Task<bool> DeleteItemAsync(int id);

    Task<bool> DeleteAlbumAsync(int id);
}