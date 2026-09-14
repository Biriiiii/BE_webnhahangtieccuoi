using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Halls;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class HallService : IHallService
{
    private readonly AppDbContext _db;

    public HallService(AppDbContext db)
    {
        _db = db;
    }

    // =========================
    // PUBLIC - GET ALL ACTIVE
    // =========================
    public async Task<List<HallDto>> GetActiveHallsAsync()
    {
        return await _db.Halls
            .Where(h => h.IsActive)
            .OrderBy(h => h.Id)
            .Select(h => new HallDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                MinTables = h.MinTables,
                MaxTables = h.MaxTables,
                AreaSqm = h.AreaSqm,
                ThumbnailUrl = h.ThumbnailUrl
            })
            .ToListAsync();
    }

    // =========================
    // PUBLIC - GET BY ID
    // =========================
    public async Task<HallDto?> GetActiveHallByIdAsync(int id)
    {
        return await _db.Halls
            .Where(h => h.Id == id && h.IsActive)
            .Select(h => new HallDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                MinTables = h.MinTables,
                MaxTables = h.MaxTables,
                AreaSqm = h.AreaSqm,
                ThumbnailUrl = h.ThumbnailUrl
            })
            .FirstOrDefaultAsync();
    }

    // =========================
    // ADMIN - GET ALL
    // =========================
    public async Task<List<Hall>> GetAllHallsAsync()
    {
        return await _db.Halls
            .OrderBy(h => h.Id)
            .ToListAsync();
    }

    // =========================
    // ADMIN - CREATE
    // =========================
    public async Task<Hall> CreateHallAsync(
        CreateUpdateHallDto dto)
    {
        var entity = new Hall
        {
            Name = dto.Name,
            Description = dto.Description,
            MinTables = dto.MinTables,
            MaxTables = dto.MaxTables,
            AreaSqm = dto.AreaSqm,
            ThumbnailUrl = dto.ThumbnailUrl,
            IsActive = dto.IsActive
        };

        _db.Halls.Add(entity);

        await _db.SaveChangesAsync();

        return entity;
    }

    // =========================
    // ADMIN - UPDATE
    // =========================
    public async Task<bool> UpdateHallAsync(
        int id,
        CreateUpdateHallDto dto)
    {
        var entity = await _db.Halls.FindAsync(id);

        if (entity == null)
        {
            return false;
        }

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.MinTables = dto.MinTables;
        entity.MaxTables = dto.MaxTables;
        entity.AreaSqm = dto.AreaSqm;
        entity.ThumbnailUrl = dto.ThumbnailUrl;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();

        return true;
    }

    // =========================
    // ADMIN - DELETE
    // =========================
    public async Task<bool> DeleteHallAsync(int id)
    {
        var entity = await _db.Halls.FindAsync(id);

        if (entity == null)
        {
            return false;
        }

        _db.Halls.Remove(entity);

        await _db.SaveChangesAsync();

        return true;
    }
}