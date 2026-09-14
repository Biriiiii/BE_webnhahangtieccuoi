using BE_webnhahangtieccuoi.DTOs.Halls;
using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IHallService
{
    Task<List<HallDto>> GetActiveHallsAsync();

    Task<HallDto?> GetActiveHallByIdAsync(int id);

    Task<List<Hall>> GetAllHallsAsync();

    Task<Hall> CreateHallAsync(
        CreateUpdateHallDto dto
    );

    Task<bool> UpdateHallAsync(
        int id,
        CreateUpdateHallDto dto
    );

    Task<bool> DeleteHallAsync(int id);
}