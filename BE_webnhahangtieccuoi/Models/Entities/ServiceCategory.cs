namespace BE_webnhahangtieccuoi.Models.Entities;

// 4 nhóm dịch vụ chính:
// 1. Tiệc cưới tại trung tâm
// 2. Rạp cưới - Decor (lưu động)
// 3. Nấu đám lưu động
// 4. Âm thanh - Ánh sáng - DJ - MC - Vũ đoàn
public class ServiceCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
