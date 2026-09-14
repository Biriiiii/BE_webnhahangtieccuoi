namespace BE_webnhahangtieccuoi.Models.Entities;

// Cấu hình chung dạng key-value, quản lý qua Admin
public class SiteSetting
{
    public int Id { get; set; }
    public string Key { get; set; } = null!;   // vd: "hotline", "address", "facebook_url"
    public string? Value { get; set; }
}
