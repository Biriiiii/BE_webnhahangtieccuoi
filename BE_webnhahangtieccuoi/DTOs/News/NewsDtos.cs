namespace BE_webnhahangtieccuoi.DTOs.News;

public class NewsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public string? Summary { get; set; }
    public string Content { get; set; } = null!;
    public DateTime PublishedAt { get; set; }
}

public class CreateUpdateNewsDto
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ThumbnailUrl { get; set; }
    public string? Summary { get; set; }
    public string Content { get; set; } = null!;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public bool IsPublished { get; set; } = true;
}
