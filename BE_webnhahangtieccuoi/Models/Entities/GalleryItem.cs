namespace BE_webnhahangtieccuoi.Models.Entities;

public class GalleryItem
{
    public int Id { get; set; }
    public int GalleryAlbumId { get; set; }
    public GalleryAlbum GalleryAlbum { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;
    public string? VideoUrl { get; set; } // Youtube link (nếu có)
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
}
