using BE_webnhahangtieccuoi.Models.Enums;

namespace BE_webnhahangtieccuoi.Models.Entities;

// Yêu cầu tư vấn / đặt tiệc gửi từ website (form public, không cần đăng nhập)
public class Lead
{
    public int Id { get; set; }

    // Thông tin khách hàng
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }

    // Thông tin sự kiện
    public int ServiceCategoryId { get; set; }
    public ServiceCategory ServiceCategory { get; set; } = null!;
    public int? ServiceId { get; set; }
    public Service? Service { get; set; }

    public DateTime? EventDate { get; set; }
    public string? EventAddress { get; set; } // khu vực / địa chỉ tổ chức

    // Số lượng - áp dụng chung cho các loại dịch vụ:
    // - Tiệc cưới tại trung tâm: số bàn dự kiến
    // - Nấu đám lưu động: số mâm dự kiến
    // - Rạp - Decor / Âm thanh ánh sáng: số bàn hoặc quy mô khách mời
    public int? QuantityTables { get; set; }   // số bàn
    public int? QuantityTrays { get; set; }    // số mâm

    public string? Note { get; set; }

    // Tổng dự toán được tính lại từ danh sách món/gói đã chọn (nếu có)
    public decimal? EstimatedTotal { get; set; }

    public LeadStatus Status { get; set; } = LeadStatus.Moi;

    public int? AssignedStaffId { get; set; }
    public User? AssignedStaff { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<LeadMenuSelection> MenuSelections { get; set; } = new List<LeadMenuSelection>();
    public ICollection<LeadHistory> Histories { get; set; } = new List<LeadHistory>();
}
