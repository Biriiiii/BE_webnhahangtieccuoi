using BE_webnhahangtieccuoi.DTOs.Leads;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

// Controller PUBLIC
// Dùng cho form "Đặt lịch tư vấn / Đặt tiệc"
[ApiController]
[Route("api/leads")]
public class LeadsController : ControllerBase
{
    private readonly ILeadService _leadService;

    public LeadsController(ILeadService leadService)
    {
        _leadService = leadService;
    }

    /// <summary>
    /// Khách gửi yêu cầu tư vấn / đặt tiệc từ website.
    /// Không cần đăng nhập.
    /// </summary>
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<LeadCreatedResultDto>> CreateLead(
        CreateLeadDto dto)
    {
        try
        {
            var result =
                await _leadService.CreateLeadAsync(dto);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}