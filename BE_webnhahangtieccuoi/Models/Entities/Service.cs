namespace BE_webnhahangtieccuoi.Models.Entities;

public class Service
{
    public int Id { get; set; }
    public int ServiceCategoryId { get; set; }
    public ServiceCategory ServiceCategory { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string? Content { get; set; }
    public string? ThumbnailUrl { get; set; }
    public decimal? BasePrice { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
