using BE_webnhahangtieccuoi.DTOs.Leads;
using BE_webnhahangtieccuoi.Models.Enums;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface ILeadService
{
    Task<LeadCreatedResultDto> CreateLeadAsync(CreateLeadDto dto);

    Task<List<LeadListItemDto>> GetAllAsync(
        LeadStatus? status,
        int? serviceCategoryId,
        DateTime? fromDate,
        DateTime? toDate);

    Task<LeadDetailDto?> GetByIdAsync(int id);

    Task<bool> UpdateStatusAsync(
        int id,
        UpdateLeadStatusDto dto,
        int? currentUserId);

    Task<bool> AssignStaffAsync(
        int id,
        AssignLeadStaffDto dto);
}