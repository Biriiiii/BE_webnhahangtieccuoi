namespace BE_webnhahangtieccuoi.DTOs.Gallery;

public class GalleryAlbumDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public List<GalleryItemDto> Items { get; set; } = new();
}

public class GalleryItemDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public string? Caption { get; set; }
}

public class CreateUpdateGalleryAlbumDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int? ServiceCategoryId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateUpdateGalleryItemDto
{
    public int GalleryAlbumId { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
}
