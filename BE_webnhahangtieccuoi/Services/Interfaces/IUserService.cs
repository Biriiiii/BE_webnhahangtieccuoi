using BE_webnhahangtieccuoi.Models.Enums;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IUserService
{
    Task<List<AdminUserDto>> GetAllAsync();

    Task<AdminUserDto> CreateAsync(
        CreateAdminUserDto dto);

    Task<bool> DeactivateAsync(int id);
}

public class CreateAdminUserDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public UserRole Role { get; set; } = UserRole.Staff;
}

public class AdminUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}