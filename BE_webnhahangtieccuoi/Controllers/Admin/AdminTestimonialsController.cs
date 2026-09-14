using BE_webnhahangtieccuoi.DTOs.Testimonials;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers.Admin;

[ApiController]
[Route("api/admin/testimonials")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminTestimonialsController : ControllerBase
{
    private readonly ITestimonialService _testimonialService;

    public AdminTestimonialsController(
        ITestimonialService testimonialService)
    {
        _testimonialService = testimonialService;
    }


    [HttpGet]
    public async Task<ActionResult<List<Testimonial>>> GetAll()
    {
        var data =
            await _testimonialService.GetAllTestimonialsAsync();

        return Ok(data);
    }


    [HttpPost]
    public async Task<ActionResult<Testimonial>> Create(
        CreateUpdateTestimonialDto dto)
    {
        var entity =
            await _testimonialService.CreateTestimonialAsync(dto);

        return Ok(entity);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _testimonialService.DeleteTestimonialAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Đánh giá không tồn tại."
            });
        }

        return NoContent();
    }
}