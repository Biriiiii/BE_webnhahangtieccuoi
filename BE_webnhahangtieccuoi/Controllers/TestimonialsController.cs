using BE_webnhahangtieccuoi.DTOs.Testimonials;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/testimonials")]
public class TestimonialsController : ControllerBase
{
    private readonly ITestimonialService _testimonialService;

    public TestimonialsController(
        ITestimonialService testimonialService)
    {
        _testimonialService = testimonialService;
    }


    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<TestimonialDto>>> GetAll()
    {
        var data =
            await _testimonialService
                .GetActiveTestimonialsAsync();

        return Ok(data);
    }
}