namespace BE_webnhahangtieccuoi.DTOs.Services;

public class ServiceCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int DisplayOrder { get; set; }
}

public class ServiceDto
{
    public int Id { get; set; }
    public int ServiceCategoryId { get; set; }
    public string ServiceCategoryName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string? Content { get; set; }
    public string? ThumbnailUrl { get; set; }
    public decimal? BasePrice { get; set; }
}

public class CreateUpdateServiceDto
{
    public int ServiceCategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string? Content { get; set; }
    public string? ThumbnailUrl { get; set; }
    public decimal? BasePrice { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}

public class CreateUpdateServiceCategoryDto
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
