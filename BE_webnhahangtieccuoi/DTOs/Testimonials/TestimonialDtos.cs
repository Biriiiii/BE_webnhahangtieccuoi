namespace BE_webnhahangtieccuoi.DTOs.Testimonials;

public class TestimonialDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public string? EventImageUrl { get; set; }
    public string Content { get; set; } = null!;
    public int Rating { get; set; }
}

public class CreateUpdateTestimonialDto
{
    public string CustomerName { get; set; } = null!;
    public string? EventImageUrl { get; set; }
    public string Content { get; set; } = null!;
    public int Rating { get; set; } = 5;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
}
