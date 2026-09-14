using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Leads;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Models.Enums;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class LeadService : ILeadService
{
    private readonly AppDbContext _db;
    private readonly IEmailService _emailService;
    private readonly ILogger<LeadService> _logger;

    public LeadService(
        AppDbContext db,
        IEmailService emailService,
        ILogger<LeadService> logger)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
    }

    // =========================================================
    // PUBLIC - TẠO LEAD
    // =========================================================
    public async Task<LeadCreatedResultDto> CreateLeadAsync(CreateLeadDto dto)
    {
        // Kiểm tra loại dịch vụ
        var serviceCategory = await _db.ServiceCategories
            .FindAsync(dto.ServiceCategoryId);

        if (serviceCategory == null)
        {
            throw new ArgumentException("Loại dịch vụ không hợp lệ.");
        }

        // Kiểm tra thông tin bắt buộc
        if (string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Phone))
        {
            throw new ArgumentException(
                "Vui lòng nhập họ tên và số điện thoại.");
        }

        var lead = new Lead
        {
            FullName = dto.FullName.Trim(),
            Phone = dto.Phone.Trim(),
            Email = dto.Email?.Trim(),

            ServiceCategoryId = dto.ServiceCategoryId,
            ServiceId = dto.ServiceId,

            EventDate = dto.EventDate,
            EventAddress = dto.EventAddress,

            QuantityTables = dto.QuantityTables,
            QuantityTrays = dto.QuantityTrays,

            Note = dto.Note,

            Status = LeadStatus.Moi,
            CreatedAt = DateTime.UtcNow
        };

        // =====================================================
        // XỬ LÝ MÓN / GÓI MÂM
        // Snapshot đơn giá tại thời điểm khách đặt
        // =====================================================

        decimal estimatedTotal = 0;

        if (dto.MenuSelections != null &&
            dto.MenuSelections.Any())
        {
            foreach (var sel in dto.MenuSelections)
            {
                if (sel.MenuItemId == null &&
                    sel.MenuPackageId == null)
                {
                    continue;
                }

                decimal unitPrice = 0;

                // Món ăn
                if (sel.MenuItemId.HasValue)
                {
                    var menuItem = await _db.MenuItems
                        .FindAsync(sel.MenuItemId.Value);

                    if (menuItem == null)
                        continue;

                    unitPrice = menuItem.UnitPrice;
                }

                // Gói mâm
                else if (sel.MenuPackageId.HasValue)
                {
                    var package = await _db.MenuPackages
                        .FindAsync(sel.MenuPackageId.Value);

                    if (package == null)
                        continue;

                    unitPrice = package.PricePerTray;
                }

                var quantity = sel.Quantity <= 0
                    ? 1
                    : sel.Quantity;

                var lineTotal = unitPrice * quantity;

                estimatedTotal += lineTotal;

                lead.MenuSelections.Add(new LeadMenuSelection
                {
                    MenuItemId = sel.MenuItemId,
                    MenuPackageId = sel.MenuPackageId,

                    Quantity = quantity,

                    UnitPrice = unitPrice,
                    LineTotal = lineTotal,

                    Note = sel.Note
                });
            }
        }

        lead.EstimatedTotal =
            estimatedTotal > 0
                ? estimatedTotal
                : null;

        // =====================================================
        // LỊCH SỬ
        // =====================================================

        lead.Histories.Add(new LeadHistory
        {
            OldStatus = null,
            NewStatus = LeadStatus.Moi,

            Note = "Yêu cầu được tạo từ website.",

            CreatedAt = DateTime.UtcNow
        });

        // =====================================================
        // LƯU DATABASE
        // =====================================================

        _db.Leads.Add(lead);

        await _db.SaveChangesAsync();

        // =====================================================
        // LOAD DATA LIÊN QUAN ĐỂ GỬI EMAIL
        // =====================================================

        await _db.Entry(lead)
            .Reference(l => l.ServiceCategory)
            .LoadAsync();

        if (lead.ServiceId.HasValue)
        {
            await _db.Entry(lead)
                .Reference(l => l.Service)
                .LoadAsync();
        }

        await _db.Entry(lead)
            .Collection(l => l.MenuSelections)
            .Query()
            .Include(s => s.MenuItem)
            .Include(s => s.MenuPackage)
            .LoadAsync();

        // =====================================================
        // GỬI EMAIL
        // =====================================================

        try
        {
            await _emailService
                .SendLeadConfirmationToCustomerAsync(lead);

            await _emailService
                .SendLeadNotificationToStaffAsync(lead);
        }
        catch (Exception ex)
        {
            // Email lỗi không làm mất Lead đã lưu
            _logger.LogError(
                ex,
                "Không thể gửi email cho Lead {LeadId}",
                lead.Id);
        }

        // =====================================================
        // TRẢ KẾT QUẢ
        // =====================================================

        return new LeadCreatedResultDto
        {
            LeadId = lead.Id,

            FullName = lead.FullName,

            ServiceCategoryName =
                lead.ServiceCategory.Name,

            EventDate = lead.EventDate,

            QuantityTables =
                lead.QuantityTables,

            QuantityTrays =
                lead.QuantityTrays,

            EstimatedTotal =
                lead.EstimatedTotal,

            MenuSelections =
                lead.MenuSelections
                    .Select(s => new LeadMenuSelectionItemDto
                    {
                        ItemName =
                            s.MenuItem?.Name
                            ?? s.MenuPackage?.Name
                            ?? "",

                        Quantity = s.Quantity,

                        UnitPrice = s.UnitPrice,

                        LineTotal = s.LineTotal,

                        Note = s.Note
                    })
                    .ToList()
        };
    }


    // =========================================================
    // ADMIN - LẤY DANH SÁCH LEAD
    // =========================================================

    public async Task<List<LeadListItemDto>> GetAllAsync(
        LeadStatus? status,
        int? serviceCategoryId,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var query = _db.Leads
            .Include(l => l.ServiceCategory)
            .Include(l => l.AssignedStaff)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(
                l => l.Status == status.Value);
        }

        if (serviceCategoryId.HasValue)
        {
            query = query.Where(
                l => l.ServiceCategoryId ==
                     serviceCategoryId.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(
                l => l.CreatedAt >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(
                l => l.CreatedAt <= toDate.Value);
        }

        return await query
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LeadListItemDto
            {
                Id = l.Id,

                FullName = l.FullName,

                Phone = l.Phone,

                Email = l.Email,

                ServiceCategoryName =
                    l.ServiceCategory.Name,

                EventDate = l.EventDate,

                QuantityTables =
                    l.QuantityTables,

                QuantityTrays =
                    l.QuantityTrays,

                EstimatedTotal =
                    l.EstimatedTotal,

                Status = l.Status,

                AssignedStaffName =
                    l.AssignedStaff != null
                        ? l.AssignedStaff.FullName
                        : null,

                CreatedAt = l.CreatedAt
            })
            .ToListAsync();
    }


    // =========================================================
    // ADMIN - CHI TIẾT LEAD
    // =========================================================

    public async Task<LeadDetailDto?> GetByIdAsync(int id)
    {
        var lead = await _db.Leads
            .Include(l => l.ServiceCategory)
            .Include(l => l.AssignedStaff)

            .Include(l => l.MenuSelections)
                .ThenInclude(s => s.MenuItem)

            .Include(l => l.MenuSelections)
                .ThenInclude(s => s.MenuPackage)

            .Include(l => l.Histories)
                .ThenInclude(h => h.ChangedByUser)

            .FirstOrDefaultAsync(l => l.Id == id);

        if (lead == null)
            return null;

        return new LeadDetailDto
        {
            Id = lead.Id,

            FullName = lead.FullName,

            Phone = lead.Phone,

            Email = lead.Email,

            ServiceCategoryName =
                lead.ServiceCategory.Name,

            EventDate = lead.EventDate,

            EventAddress =
                lead.EventAddress,

            QuantityTables =
                lead.QuantityTables,

            QuantityTrays =
                lead.QuantityTrays,

            EstimatedTotal =
                lead.EstimatedTotal,

            Note = lead.Note,

            Status =
                lead.Status,

            AssignedStaffName =
                lead.AssignedStaff?.FullName,

            CreatedAt =
                lead.CreatedAt,

            MenuSelections =
                lead.MenuSelections
                    .Select(s => new LeadMenuSelectionItemDto
                    {
                        ItemName =
                            s.MenuItem?.Name
                            ?? s.MenuPackage?.Name
                            ?? "",

                        Quantity =
                            s.Quantity,

                        UnitPrice =
                            s.UnitPrice,

                        LineTotal =
                            s.LineTotal,

                        Note =
                            s.Note
                    })
                    .ToList(),

            Histories =
                lead.Histories
                    .OrderByDescending(h => h.CreatedAt)
                    .Select(h => new LeadHistoryItemDto
                    {
                        OldStatus =
                            h.OldStatus,

                        NewStatus =
                            h.NewStatus,

                        Note =
                            h.Note,

                        ChangedByUserName =
                            h.ChangedByUser?.FullName,

                        CreatedAt =
                            h.CreatedAt
                    })
                    .ToList()
        };
    }


    // =========================================================
    // ADMIN - CẬP NHẬT TRẠNG THÁI
    // =========================================================

    public async Task<bool> UpdateStatusAsync(
        int id,
        UpdateLeadStatusDto dto,
        int? currentUserId)
    {
        var lead = await _db.Leads
            .FindAsync(id);

        if (lead == null)
            return false;

        var oldStatus = lead.Status;

        lead.Status = dto.Status;

        lead.UpdatedAt =
            DateTime.UtcNow;

        _db.LeadHistories.Add(
            new LeadHistory
            {
                LeadId = lead.Id,

                OldStatus = oldStatus,

                NewStatus = dto.Status,

                Note = dto.Note,

                ChangedByUserId =
                    currentUserId,

                CreatedAt =
                    DateTime.UtcNow
            });

        await _db.SaveChangesAsync();

        return true;
    }


    // =========================================================
    // ADMIN - GÁN NHÂN VIÊN
    // =========================================================

    public async Task<bool> AssignStaffAsync(
        int id,
        AssignLeadStaffDto dto)
    {
        var lead = await _db.Leads
            .FindAsync(id);

        if (lead == null)
            return false;

        var staff = await _db.Users
            .FindAsync(dto.StaffUserId);

        if (staff == null)
        {
            throw new KeyNotFoundException(
                "Nhân viên không tồn tại.");
        }

        lead.AssignedStaffId =
            dto.StaffUserId;

        lead.UpdatedAt =
            DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return true;
    }
}