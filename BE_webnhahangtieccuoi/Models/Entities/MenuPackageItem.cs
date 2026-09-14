namespace BE_webnhahangtieccuoi.Models.Entities;

// Bảng nối: món nào thuộc gói mâm nào
public class MenuPackageItem
{
    public int Id { get; set; }
    public int MenuPackageId { get; set; }
    public MenuPackage MenuPackage { get; set; } = null!;
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;
}
