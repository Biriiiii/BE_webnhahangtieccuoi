namespace BE_webnhahangtieccuoi.DTOs.Halls;

public class HallDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int MinTables { get; set; }
    public int MaxTables { get; set; }
    public decimal? AreaSqm { get; set; }
    public string? ThumbnailUrl { get; set; }
}

public class CreateUpdateHallDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int MinTables { get; set; }
    public int MaxTables { get; set; }
    public decimal? AreaSqm { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
