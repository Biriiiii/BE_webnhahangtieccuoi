namespace BE_webnhahangtieccuoi.Models.Entities;

public class GalleryAlbum
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public int? ServiceCategoryId { get; set; }
    public ServiceCategory? ServiceCategory { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GalleryItem> Items { get; set; } = new List<GalleryItem>();
}
