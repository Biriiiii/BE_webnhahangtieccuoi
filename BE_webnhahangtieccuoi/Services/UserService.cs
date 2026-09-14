using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }


    // =========================================================
    // GET ALL USERS
    // =========================================================

    public async Task<List<AdminUserDto>> GetAllAsync()
    {
        return await _db.Users
            .OrderBy(u => u.Id)
            .Select(u => new AdminUserDto
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                IsActive = u.IsActive
            })
            .ToListAsync();
    }


    // =========================================================
    // CREATE USER
    // =========================================================

    public async Task<AdminUserDto> CreateAsync(
        CreateAdminUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
        {
            throw new ArgumentException(
                "Tên đăng nhập không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new ArgumentException(
                "Mật khẩu không được để trống.");
        }

        if (string.IsNullOrWhiteSpace(dto.FullName))
        {
            throw new ArgumentException(
                "Họ tên không được để trống.");
        }


        var usernameExists =
            await _db.Users.AnyAsync(
                u => u.Username == dto.Username);

        if (usernameExists)
        {
            throw new InvalidOperationException(
                "Tên đăng nhập đã tồn tại.");
        }


        var entity = new User
        {
            Username = dto.Username.Trim(),

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    dto.Password),

            FullName = dto.FullName.Trim(),

            Email = dto.Email?.Trim(),

            Phone = dto.Phone?.Trim(),

            Role = dto.Role,

            IsActive = true,

            CreatedAt = DateTime.UtcNow
        };


        _db.Users.Add(entity);

        await _db.SaveChangesAsync();


        return new AdminUserDto
        {
            Id = entity.Id,
            Username = entity.Username,
            FullName = entity.FullName,
            Email = entity.Email,
            Role = entity.Role,
            IsActive = entity.IsActive
        };
    }


    // =========================================================
    // DEACTIVATE USER
    // =========================================================

    public async Task<bool> DeactivateAsync(int id)
    {
        var entity =
            await _db.Users.FindAsync(id);

        if (entity == null)
            return false;


        entity.IsActive = false;

        await _db.SaveChangesAsync();

        return true;
    }
}