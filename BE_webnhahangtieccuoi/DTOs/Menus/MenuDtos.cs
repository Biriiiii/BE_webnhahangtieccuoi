namespace BE_webnhahangtieccuoi.DTOs.Menus;

public class MenuGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<MenuItemDto> MenuItems { get; set; } = new();
    public List<MenuPackageDto> MenuPackages { get; set; } = new();
}

public class MenuItemDto
{
    public int Id { get; set; }
    public int MenuGroupId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
}

public class MenuPackageDto
{
    public int Id { get; set; }
    public int MenuGroupId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PricePerTray { get; set; }
    public List<string> IncludedItemNames { get; set; } = new();
}

public class CreateUpdateMenuItemDto
{
    public int MenuGroupId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateUpdateMenuPackageDto
{
    public int MenuGroupId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal PricePerTray { get; set; }
    public bool IsActive { get; set; } = true;
    public List<int> MenuItemIds { get; set; } = new();
}

public class CreateUpdateMenuGroupDto
{
    public int? ServiceCategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
