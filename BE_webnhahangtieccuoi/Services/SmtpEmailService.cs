using System.Text;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BE_webnhahangtieccuoi.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendLeadConfirmationToCustomerAsync(Lead lead)
    {
        if (string.IsNullOrWhiteSpace(lead.Email))
        {
            _logger.LogInformation("Lead {LeadId} không có email, bỏ qua gửi mail xác nhận cho khách.", lead.Id);
            return;
        }

        var subject = $"[Xác nhận yêu cầu #{lead.Id}] Cảm ơn {lead.FullName} đã liên hệ";
        var body = BuildLeadEmailBody(lead, isForStaff: false);

        await SendAsync(lead.Email!, subject, body);
    }

    public async Task SendLeadNotificationToStaffAsync(Lead lead)
    {
        var staffEmails = _config.GetSection("EmailSettings:StaffNotificationEmails").Get<string[]>() ?? Array.Empty<string>();
        if (staffEmails.Length == 0)
        {
            _logger.LogWarning("Chưa cấu hình StaffNotificationEmails, không gửi được email thông báo nội bộ cho Lead {LeadId}.", lead.Id);
            return;
        }

        var subject = $"[YÊU CẦU ĐẶT TIỆC MỚI #{lead.Id}] {lead.FullName} - {lead.ServiceCategory.Name}";
        var body = BuildLeadEmailBody(lead, isForStaff: true);

        foreach (var email in staffEmails)
        {
            await SendAsync(email, subject, body);
        }
    }

    // Xây dựng nội dung email HTML, luôn hiển thị đầy đủ:
    // - Số bàn / số mâm khách yêu cầu
    // - Danh sách món/gói mâm đã chọn kèm số lượng, đơn giá, thành tiền
    // - Tổng dự toán
    private string BuildLeadEmailBody(Lead lead, bool isForStaff)
    {
        var sb = new StringBuilder();
        sb.Append("<div style='font-family:Arial,sans-serif;font-size:14px;color:#333;line-height:1.6'>");

        if (isForStaff)
        {
            sb.Append("<h2 style='color:#c0392b'>Có yêu cầu đặt tiệc mới từ website</h2>");
        }
        else
        {
            sb.Append($"<h2>Cảm ơn {lead.FullName} đã gửi yêu cầu tư vấn!</h2>");
            sb.Append("<p>Trung tâm tiệc cưới đã nhận được thông tin của bạn và sẽ liên hệ lại trong thời gian sớm nhất. Dưới đây là chi tiết yêu cầu:</p>");
        }

        sb.Append("<table style='border-collapse:collapse;width:100%;max-width:600px'>");
        sb.Append(RowHtml("Mã yêu cầu", $"#{lead.Id}"));
        sb.Append(RowHtml("Họ tên khách hàng", lead.FullName));
        sb.Append(RowHtml("Số điện thoại", lead.Phone));
        if (!string.IsNullOrWhiteSpace(lead.Email))
            sb.Append(RowHtml("Email", lead.Email!));
        sb.Append(RowHtml("Loại dịch vụ", lead.ServiceCategory.Name));
        if (lead.Service != null)
            sb.Append(RowHtml("Gói dịch vụ quan tâm", lead.Service.Name));
        if (lead.EventDate.HasValue)
            sb.Append(RowHtml("Ngày dự kiến tổ chức", lead.EventDate.Value.ToString("dd/MM/yyyy")));
        if (!string.IsNullOrWhiteSpace(lead.EventAddress))
            sb.Append(RowHtml("Khu vực / địa chỉ tổ chức", lead.EventAddress!));

        // ==== SỐ LƯỢNG - hiển thị nổi bật theo yêu cầu ====
        if (lead.QuantityTables.HasValue)
            sb.Append(RowHtml("Số lượng bàn dự kiến", $"<b>{lead.QuantityTables.Value} bàn</b>"));
        if (lead.QuantityTrays.HasValue)
            sb.Append(RowHtml("Số lượng mâm dự kiến", $"<b>{lead.QuantityTrays.Value} mâm</b>"));

        if (!string.IsNullOrWhiteSpace(lead.Note))
            sb.Append(RowHtml("Ghi chú", lead.Note!));

        sb.Append("</table>");

        // ==== Danh sách món / gói mâm đã chọn (nếu có) ====
        if (lead.MenuSelections.Any())
        {
            sb.Append("<h3 style='margin-top:20px'>Thực đơn / gói mâm đã chọn</h3>");
            sb.Append("<table style='border-collapse:collapse;width:100%;max-width:600px' border='1' cellpadding='8'>");
            sb.Append("<tr style='background:#f5f5f5'>"
                + "<th align='left'>Tên món / gói</th>"
                + "<th align='right'>Số lượng</th>"
                + "<th align='right'>Đơn giá</th>"
                + "<th align='right'>Thành tiền</th>"
                + "</tr>");

            foreach (var item in lead.MenuSelections)
            {
                var itemName = item.MenuItem?.Name ?? item.MenuPackage?.Name ?? "(Không xác định)";
                sb.Append("<tr>");
                sb.Append($"<td>{itemName}{(string.IsNullOrWhiteSpace(item.Note) ? "" : $"<br/><i style='color:#777'>Ghi chú: {item.Note}</i>")}</td>");
                sb.Append($"<td align='right'>{item.Quantity}</td>");
                sb.Append($"<td align='right'>{item.UnitPrice:N0} đ</td>");
                sb.Append($"<td align='right'>{item.LineTotal:N0} đ</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
        }

        if (lead.EstimatedTotal.HasValue)
        {
            sb.Append($"<p style='margin-top:15px;font-size:16px'><b>Tổng dự toán: {lead.EstimatedTotal.Value:N0} đ</b></p>");
            sb.Append("<p style='font-size:12px;color:#777'>* Đây là số tiền tạm tính dựa trên lựa chọn của quý khách, chưa bao gồm các chi phí phát sinh (nếu có). Nhân viên tư vấn sẽ liên hệ để chốt báo giá chính thức.</p>");
        }

        if (isForStaff)
        {
            sb.Append("<p style='margin-top:20px;color:#c0392b'><b>Vui lòng vào trang Quản trị để tiếp nhận và xử lý yêu cầu này.</b></p>");
        }
        else
        {
            sb.Append("<p style='margin-top:20px'>Trân trọng cảm ơn quý khách!</p>");
        }

        sb.Append("</div>");
        return sb.ToString();
    }

    private static string RowHtml(string label, string value)
    {
        return $"<tr><td style='padding:4px 8px;color:#666;white-space:nowrap'>{label}</td><td style='padding:4px 8px'>{value}</td></tr>";
    }

    private async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        var emailSection = _config.GetSection("EmailSettings");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(emailSection["SenderName"], emailSection["SenderEmail"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        try
        {
            using var client = new SmtpClient();
            var host = emailSection["SmtpHost"]!;
            var port = int.Parse(emailSection["SmtpPort"] ?? "587");
            var useSsl = bool.Parse(emailSection["UseSsl"] ?? "true");

            await client.ConnectAsync(host, port, useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            await client.AuthenticateAsync(emailSection["Username"], emailSection["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // Không throw để việc gửi mail lỗi không làm hỏng luồng tạo Lead;
            // chỉ log lại để kiểm tra cấu hình SMTP.
            _logger.LogError(ex, "Gửi email tới {ToEmail} thất bại.", toEmail);
        }
    }
}
