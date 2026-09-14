namespace BE_webnhahangtieccuoi.Models.Entities;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public string? Summary { get; set; }
    public string Content { get; set; } = null!;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
