namespace BE_webnhahangtieccuoi.Models.Entities;

public class Testimonial
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public string? EventImageUrl { get; set; }
    public string Content { get; set; } = null!;
    public int Rating { get; set; } = 5; // 1-5 sao
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
