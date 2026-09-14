namespace BE_webnhahangtieccuoi.Models.Entities;

public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int MinTables { get; set; }
    public int MaxTables { get; set; }
    public decimal? AreaSqm { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
