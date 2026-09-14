using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Testimonials;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class TestimonialService : ITestimonialService
{
    private readonly AppDbContext _db;

    public TestimonialService(AppDbContext db)
    {
        _db = db;
    }


    // =========================================================
    // PUBLIC
    // =========================================================

    public async Task<List<TestimonialDto>>
        GetActiveTestimonialsAsync()
    {
        return await _db.Testimonials
            .Where(t => t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .Select(t => new TestimonialDto
            {
                Id = t.Id,
                CustomerName = t.CustomerName,
                EventImageUrl = t.EventImageUrl,
                Content = t.Content,
                Rating = t.Rating
            })
            .ToListAsync();
    }

    // =========================================================
    // ADMIN
    // =========================================================

    public async Task<List<Testimonial>>
        GetAllTestimonialsAsync()
    {
        return await _db.Testimonials
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
    }


    public async Task<Testimonial>
        CreateTestimonialAsync(
            CreateUpdateTestimonialDto dto)
    {
        var entity = new Testimonial
        {
            CustomerName = dto.CustomerName,
            EventImageUrl = dto.EventImageUrl,
            Content = dto.Content,
            Rating = dto.Rating,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        _db.Testimonials.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }


    public async Task<bool>
        DeleteTestimonialAsync(int id)
    {
        var entity =
            await _db.Testimonials.FindAsync(id);

        if (entity == null)
            return false;

        _db.Testimonials.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }
}