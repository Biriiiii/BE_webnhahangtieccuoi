namespace BE_webnhahangtieccuoi.Models.Entities;

// Nhóm thực đơn, ví dụ: "Thực đơn cưới trung tâm", "Thực đơn cỗ mâm lưu động"
public class MenuGroup
{
    public int Id { get; set; }
    public int? ServiceCategoryId { get; set; }
    public ServiceCategory? ServiceCategory { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public ICollection<MenuPackage> MenuPackages { get; set; } = new List<MenuPackage>();
}
