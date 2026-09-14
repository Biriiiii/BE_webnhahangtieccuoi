using BE_webnhahangtieccuoi.DTOs.Leads;
using BE_webnhahangtieccuoi.Models.Enums;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

// Dành cho Admin/Staff quản lý các yêu cầu đặt tiệc
[ApiController]
[Route("api/admin/leads")]
[Authorize(Roles = "SuperAdmin,Admin,Staff")]
public class AdminLeadsController : ControllerBase
{
    private readonly ILeadService _leadService;

    public AdminLeadsController(
        ILeadService leadService)
    {
        _leadService = leadService;
    }


    // =========================================================
    // GET ALL
    // =========================================================

    [HttpGet]
    public async Task<ActionResult<List<LeadListItemDto>>> GetAll(
        [FromQuery] LeadStatus? status,
        [FromQuery] int? serviceCategoryId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var data =
            await _leadService.GetAllAsync(
                status,
                serviceCategoryId,
                fromDate,
                toDate);

        return Ok(data);
    }


    // =========================================================
    // GET DETAIL
    // =========================================================

    [HttpGet("{id}")]
    public async Task<ActionResult<LeadDetailDto>> GetById(
        int id)
    {
        var data =
            await _leadService.GetByIdAsync(id);

        if (data == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy yêu cầu."
            });
        }

        return Ok(data);
    }


    // =========================================================
    // UPDATE STATUS
    // =========================================================

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        UpdateLeadStatusDto dto)
    {
        // JWT hiện tại có claim "sub" chứa User.Id
        var userIdClaim =
            User.FindFirst("sub")?.Value;

        int? currentUserId = null;

        if (int.TryParse(
            userIdClaim,
            out var userId))
        {
            currentUserId = userId;
        }

        var result =
            await _leadService.UpdateStatusAsync(
                id,
                dto,
                currentUserId);

        if (!result)
        {
            return NotFound(new
            {
                message = "Không tìm thấy yêu cầu."
            });
        }

        return NoContent();
    }


    // =========================================================
    // ASSIGN STAFF
    // =========================================================

    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignStaff(
        int id,
        AssignLeadStaffDto dto)
    {
        try
        {
            var result =
                await _leadService.AssignStaffAsync(
                    id,
                    dto);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy yêu cầu."
                });
            }

            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}