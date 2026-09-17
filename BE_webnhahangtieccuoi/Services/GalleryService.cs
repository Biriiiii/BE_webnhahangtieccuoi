using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Gallery;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class GalleryService : IGalleryService
{
    private readonly AppDbContext _db;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IConfiguration _configuration;

    public GalleryService(
        AppDbContext db,
        ICloudinaryService cloudinaryService,
        IConfiguration configuration)
    {
        _db = db;
        _cloudinaryService = cloudinaryService;
        _configuration = configuration;
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

                // Google Drive
                GoogleDriveFolderId = a.GoogleDriveFolderId,

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

            // Google Drive
            GoogleDriveFolderId = dto.GoogleDriveFolderId,

            CreatedAt = DateTime.UtcNow
        };

        _db.GalleryAlbums.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    // =========================
    // ADMIN - ADD 1 ITEM
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
    // ADMIN - UPLOAD NHIỀU ẢNH
    // =========================
    public async Task<List<GalleryItem>> AddItemsAsync(
        int galleryAlbumId,
        List<IFormFile> files,
        string? caption = null)
    {
        var album = await _db.GalleryAlbums
            .FirstOrDefaultAsync(a => a.Id == galleryAlbumId);

        if (album == null)
        {
            throw new KeyNotFoundException(
                "Gallery album không tồn tại."
            );
        }

        if (files == null || files.Count == 0)
        {
            throw new ArgumentException(
                "Vui lòng chọn ít nhất một ảnh."
            );
        }

        var maxDisplayOrder = await _db.GalleryItems
            .Where(i => i.GalleryAlbumId == galleryAlbumId)
            .Select(i => (int?)i.DisplayOrder)
            .MaxAsync() ?? 0;

        var items = new List<GalleryItem>();

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            var allowedExtensions = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException(
                    $"File '{file.FileName}' không phải định dạng ảnh hợp lệ."
                );
            }

            var imageUrl =
                await _cloudinaryService.UploadImageAsync(
                    file,
                    $"wedding-center/gallery/{galleryAlbumId}"
                );

            maxDisplayOrder++;

            var item = new GalleryItem
            {
                GalleryAlbumId = galleryAlbumId,
                ImageUrl = imageUrl,
                Caption = caption,
                DisplayOrder = maxDisplayOrder
            };

            items.Add(item);
        }

        await _db.GalleryItems.AddRangeAsync(items);

        await _db.SaveChangesAsync();

        if (string.IsNullOrWhiteSpace(album.CoverImageUrl) &&
            items.Count > 0)
        {
            album.CoverImageUrl = items[0].ImageUrl;

            await _db.SaveChangesAsync();
        }

        return items;
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

    // =========================
    // ADMIN - SYNC GOOGLE DRIVE
    // =========================
    public async Task<(int AddedCount, int TotalCount)>
        SyncGoogleDriveFolderAsync(int galleryAlbumId)
    {
        // =========================
        // 1. LẤY ALBUM
        // =========================
        var album = await _db.GalleryAlbums
            .Include(a => a.Items)
            .FirstOrDefaultAsync(a => a.Id == galleryAlbumId);

        if (album == null)
        {
            throw new KeyNotFoundException(
                "Gallery album không tồn tại."
            );
        }

        // =========================
        // 2. KIỂM TRA FOLDER ID
        // =========================
        if (string.IsNullOrWhiteSpace(album.GoogleDriveFolderId))
        {
            throw new ArgumentException(
                "Album này chưa được nhập Folder ID hoặc Link Google Drive."
            );
        }

        // =========================
        // 3. LẤY FOLDER ID
        // =========================
        var rawFolderId = album.GoogleDriveFolderId.Trim();

        var matchFolder =
            System.Text.RegularExpressions.Regex.Match(
                rawFolderId,
                @"folders/([a-zA-Z0-9_-]+)"
            );

        var folderId = matchFolder.Success
            ? matchFolder.Groups[1].Value
            : rawFolderId;

        // =========================
        // 4. GOOGLE DRIVE API KEY
        // =========================
        var apiKey = _configuration["GoogleDrive:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "Chưa cấu hình GoogleDrive:ApiKey trong appsettings.json."
            );
        }

        // =========================
        // 5. GỌI GOOGLE DRIVE API
        // =========================
        using var client = new HttpClient();

        var allFiles =
            new List<(string Id, string Name, string MimeType)>();

        string? nextPageToken = null;

        do
        {
            var query =
                $"'{folderId}' in parents and trashed = false";

            var fields =
                "nextPageToken,files(id,name,mimeType)";

            var url =
                "https://www.googleapis.com/drive/v3/files" +
                $"?q={Uri.EscapeDataString(query)}" +
                $"&fields={Uri.EscapeDataString(fields)}" +
                "&pageSize=100" +
                "&orderBy=name" +
                $"&key={Uri.EscapeDataString(apiKey)}";

            if (!string.IsNullOrWhiteSpace(nextPageToken))
            {
                url +=
                    $"&pageToken={Uri.EscapeDataString(nextPageToken)}";
            }

            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(url);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Không thể kết nối Google Drive API: {ex.Message}"
                );
            }

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"Google Drive API lỗi " +
                    $"({(int)response.StatusCode}): {responseBody}"
                );
            }

            using var json =
                System.Text.Json.JsonDocument.Parse(responseBody);

            // =========================
            // 6. LẤY FILE
            // =========================
            if (json.RootElement.TryGetProperty(
                "files",
                out var filesElement))
            {
                foreach (var file in filesElement.EnumerateArray())
                {
                    if (!file.TryGetProperty(
                        "id",
                        out var idElement))
                    {
                        continue;
                    }

                    if (!file.TryGetProperty(
                        "name",
                        out var nameElement))
                    {
                        continue;
                    }

                    if (!file.TryGetProperty(
                        "mimeType",
                        out var mimeTypeElement))
                    {
                        continue;
                    }

                    var fileId = idElement.GetString();
                    var fileName = nameElement.GetString();
                    var mimeType = mimeTypeElement.GetString();

                    if (string.IsNullOrWhiteSpace(fileId) ||
                        string.IsNullOrWhiteSpace(fileName) ||
                        string.IsNullOrWhiteSpace(mimeType))
                    {
                        continue;
                    }

                    // Chỉ lấy file ảnh
                    if (mimeType.StartsWith(
                        "image/",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        allFiles.Add(
                            (fileId, fileName, mimeType)
                        );
                    }
                }
            }

            // =========================
            // 7. PAGINATION
            // =========================
            if (json.RootElement.TryGetProperty(
                "nextPageToken",
                out var tokenElement))
            {
                nextPageToken = tokenElement.GetString();
            }
            else
            {
                nextPageToken = null;
            }

        } while (!string.IsNullOrWhiteSpace(nextPageToken));

        // =========================
        // 8. KHÔNG CÓ ẢNH
        // =========================
        if (allFiles.Count == 0)
        {
            throw new InvalidOperationException(
                "Không tìm thấy ảnh nào trong Folder Google Drive. " +
                "Hãy kiểm tra Folder ID và quyền " +
                "'Bất kỳ ai có đường liên kết → Người xem'."
            );
        }

        // =========================
        // 9. LẤY CÁC ẢNH ĐÃ SYNC
        // =========================
        var existingItems = album.Items.ToList();

        var existingGoogleDriveIds =
            new HashSet<string>();

        foreach (var item in existingItems)
        {
            if (string.IsNullOrWhiteSpace(item.ImageUrl))
            {
                continue;
            }

            // URL mới:
            // https://drive.google.com/thumbnail?id=FILE_ID&sz=w1200

            var match =
                System.Text.RegularExpressions.Regex.Match(
                    item.ImageUrl,
                    @"[?&]id=([a-zA-Z0-9_-]+)"
                );

            if (match.Success)
            {
                existingGoogleDriveIds.Add(
                    match.Groups[1].Value
                );
            }
        }

        // =========================
        // 10. DISPLAY ORDER
        // =========================
        var maxOrder = existingItems.Any()
            ? existingItems.Max(i => i.DisplayOrder)
            : 0;

        var newItems = new List<GalleryItem>();

        // =========================
        // 11. THÊM ẢNH MỚI
        // =========================
        foreach (var file in allFiles)
        {
            // Không thêm trùng
            if (existingGoogleDriveIds.Contains(file.Id))
            {
                continue;
            }

            maxOrder++;

            var imageUrl =
                $"https://drive.google.com/thumbnail" +
                $"?id={file.Id}&sz=w1200";

            var newItem = new GalleryItem
            {
                GalleryAlbumId = album.Id,
                ImageUrl = imageUrl,

                // Dùng tên file làm caption
                Caption = file.Name,

                DisplayOrder = maxOrder
            };

            newItems.Add(newItem);
        }

        // =========================
        // 12. SAVE DATABASE
        // =========================
        if (newItems.Count > 0)
        {
            await _db.GalleryItems.AddRangeAsync(newItems);

            if (string.IsNullOrWhiteSpace(
                album.CoverImageUrl))
            {
                album.CoverImageUrl =
                    newItems[0].ImageUrl;
            }

            await _db.SaveChangesAsync();
        }

        // =========================
        // 13. ĐẾM TỔNG ẢNH
        // =========================
        var totalCount =
            await _db.GalleryItems
                .CountAsync(
                    i => i.GalleryAlbumId == album.Id
                );

        return (
            newItems.Count,
            totalCount
        );
    }
}