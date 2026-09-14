namespace BE_webnhahangtieccuoi.Models.Entities;

// Gói mâm/set menu có sẵn, giá trọn gói theo bàn/mâm
public class MenuPackage
{
    public int Id { get; set; }
    public int MenuGroupId { get; set; }
    public MenuGroup MenuGroup { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PricePerTray { get; set; } // giá / mâm hoặc / bàn
    public bool IsActive { get; set; } = true;

    public ICollection<MenuPackageItem> MenuPackageItems { get; set; } = new List<MenuPackageItem>();
    public ICollection<LeadMenuSelection> LeadMenuSelections { get; set; } = new List<LeadMenuSelection>();
}
