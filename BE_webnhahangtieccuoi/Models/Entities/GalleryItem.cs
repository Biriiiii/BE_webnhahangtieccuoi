namespace BE_webnhahangtieccuoi.Models.Entities;
using System.Text.Json.Serialization;
public class GalleryItem
{
    public int Id { get; set; }

    public int GalleryAlbumId { get; set; }

    [JsonIgnore]
    public GalleryAlbum GalleryAlbum { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? VideoUrl { get; set; }

    public string? Caption { get; set; }

    public int DisplayOrder { get; set; }
}
