namespace BE_webnhahangtieccuoi.Models.Entities;

// Chi tiết món / gói mâm khách đã chọn khi gửi yêu cầu đặt tiệc (giống giỏ hàng)
// Dùng cho "Nấu đám lưu động" và "Tiệc cưới tại trung tâm" khi khách tự chọn thực đơn
public class LeadMenuSelection
{
    public int Id { get; set; }

    public int LeadId { get; set; }
    public Lead Lead { get; set; } = null!;

    // Chỉ 1 trong 2: chọn món lẻ hoặc chọn gói mâm có sẵn
    public int? MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    public int? MenuPackageId { get; set; }
    public MenuPackage? MenuPackage { get; set; }

    // Số lượng mâm/bàn áp dụng cho món/gói này
    public int Quantity { get; set; } = 1;

    // Đơn giá được chốt tại thời điểm khách chọn (snapshot giá, tránh đổi giá về sau làm sai lệch dữ liệu)
    public decimal UnitPrice { get; set; }

    // Thành tiền = Quantity * UnitPrice
    public decimal LineTotal { get; set; }

    public string? Note { get; set; } // vd: dị ứng, món kiêng, yêu cầu riêng cho mâm này
}
