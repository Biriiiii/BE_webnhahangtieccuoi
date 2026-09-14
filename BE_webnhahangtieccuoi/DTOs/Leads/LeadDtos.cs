using BE_webnhahangtieccuoi.Models.Enums;

namespace BE_webnhahangtieccuoi.DTOs.Leads;

// Dữ liệu khách gửi lên từ form đặt tiệc / yêu cầu tư vấn (public, không cần đăng nhập)
public class CreateLeadDto
{
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }

    public int ServiceCategoryId { get; set; }
    public int? ServiceId { get; set; }

    public DateTime? EventDate { get; set; }
    public string? EventAddress { get; set; }

    // Số lượng khách gửi lên - BẮT BUỘC hiển thị đầy đủ trong email thông báo
    public int? QuantityTables { get; set; }
    public int? QuantityTrays { get; set; }

    public string? Note { get; set; }

    // Danh sách món / gói mâm khách chọn kèm theo (áp dụng cho Nấu đám lưu động,
    // và Tiệc cưới tại trung tâm nếu khách tự chọn thực đơn theo bàn)
    public List<CreateLeadMenuSelectionDto>? MenuSelections { get; set; }
}

public class CreateLeadMenuSelectionDto
{
    public int? MenuItemId { get; set; }
    public int? MenuPackageId { get; set; }
    public int Quantity { get; set; } = 1;
    public string? Note { get; set; }
}

// Dữ liệu trả về sau khi tạo Lead thành công (hiển thị lại cho khách xem tóm tắt)
public class LeadCreatedResultDto
{
    public int LeadId { get; set; }
    public string FullName { get; set; } = null!;
    public string ServiceCategoryName { get; set; } = null!;
    public DateTime? EventDate { get; set; }
    public int? QuantityTables { get; set; }
    public int? QuantityTrays { get; set; }
    public decimal? EstimatedTotal { get; set; }
    public List<LeadMenuSelectionItemDto> MenuSelections { get; set; } = new();
}

public class LeadMenuSelectionItemDto
{
    public string ItemName { get; set; } = null!;   // tên món hoặc tên gói mâm
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public string? Note { get; set; }
}

// Danh sách Lead hiển thị trong Admin (dạng bảng/kanban)
public class LeadListItemDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public string ServiceCategoryName { get; set; } = null!;
    public DateTime? EventDate { get; set; }
    public int? QuantityTables { get; set; }
    public int? QuantityTrays { get; set; }
    public decimal? EstimatedTotal { get; set; }
    public LeadStatus Status { get; set; }
    public string? AssignedStaffName { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Chi tiết đầy đủ 1 Lead (dùng khi Admin/Staff mở xem chi tiết)
public class LeadDetailDto : LeadListItemDto
{
    public string? EventAddress { get; set; }
    public string? Note { get; set; }
    public List<LeadMenuSelectionItemDto> MenuSelections { get; set; } = new();
    public List<LeadHistoryItemDto> Histories { get; set; } = new();
}

public class LeadHistoryItemDto
{
    public LeadStatus? OldStatus { get; set; }
    public LeadStatus NewStatus { get; set; }
    public string? Note { get; set; }
    public string? ChangedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateLeadStatusDto
{
    public LeadStatus Status { get; set; }
    public string? Note { get; set; }
}

public class AssignLeadStaffDto
{
    public int StaffUserId { get; set; }
}
