namespace BE_webnhahangtieccuoi.Models.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public int MenuGroupId { get; set; }
    public MenuGroup MenuGroup { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<MenuPackageItem> MenuPackageItems { get; set; } = new List<MenuPackageItem>();
    public ICollection<LeadMenuSelection> LeadMenuSelections { get; set; } = new List<LeadMenuSelection>();
}
