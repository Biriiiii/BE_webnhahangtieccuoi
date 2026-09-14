using BE_webnhahangtieccuoi.Models.Enums;

namespace BE_webnhahangtieccuoi.Models.Entities;

// Lịch sử xử lý Lead - log mỗi lần đổi trạng thái / ghi chú CSKH
public class LeadHistory
{
    public int Id { get; set; }

    public int LeadId { get; set; }
    public Lead Lead { get; set; } = null!;

    public LeadStatus? OldStatus { get; set; }
    public LeadStatus NewStatus { get; set; }

    public string? Note { get; set; }

    public int? ChangedByUserId { get; set; }
    public User? ChangedByUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
