using BE_webnhahangtieccuoi.DTOs.News;
using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface INewsService
{
    // Public
    Task<List<NewsDto>> GetPublishedNewsAsync();
    Task<NewsDto?> GetPublishedNewsBySlugAsync(string slug);

    // Admin
    Task<List<News>> GetAllNewsAsync();
    Task<News> CreateNewsAsync(CreateUpdateNewsDto dto);
    Task<bool> UpdateNewsAsync(int id, CreateUpdateNewsDto dto);
    Task<bool> DeleteNewsAsync(int id);
}