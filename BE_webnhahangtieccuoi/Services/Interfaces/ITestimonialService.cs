using BE_webnhahangtieccuoi.DTOs.Testimonials;
using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface ITestimonialService
{
    // PUBLIC
    Task<List<TestimonialDto>> GetActiveTestimonialsAsync();

    // ADMIN
    Task<List<Testimonial>> GetAllTestimonialsAsync();

    Task<Testimonial> CreateTestimonialAsync(
        CreateUpdateTestimonialDto dto);

    Task<bool> DeleteTestimonialAsync(int id);
}