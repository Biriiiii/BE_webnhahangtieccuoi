using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// Gửi email xác nhận cho KHÁCH HÀNG sau khi gửi yêu cầu đặt tiệc/tư vấn.
    /// Nội dung mail có đầy đủ số lượng bàn/mâm, danh sách món đã chọn và tổng dự toán.
    /// </summary>
    Task SendLeadConfirmationToCustomerAsync(Lead lead);

    /// <summary>
    /// Gửi email THÔNG BÁO cho nhân viên Sale/CSKH khi có yêu cầu đặt tiệc mới.
    /// Nội dung mail có đầy đủ thông tin khách + số lượng + món đã chọn để nhân viên xử lý ngay.
    /// </summary>
    Task SendLeadNotificationToStaffAsync(Lead lead);
}
