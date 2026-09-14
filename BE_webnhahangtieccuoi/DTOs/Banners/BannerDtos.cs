namespace BE_webnhahangtieccuoi.DTOs.Banners;

public class BannerDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public string? Title { get; set; }
    public string? LinkUrl { get; set; }
}

public class CreateUpdateBannerDto
{
    public string ImageUrl { get; set; } = null!;
    public string? Title { get; set; }
    public string? LinkUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
