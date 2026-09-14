using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.DTOs.Auth;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IJwtService _jwtService;

    public AuthController(AppDbContext db, IJwtService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    // Đăng nhập cho Admin/Staff (JWT) - theo yêu cầu Phase 1 MVP
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });
        }

        var (token, expiresAt) = _jwtService.GenerateToken(user);

        return Ok(new LoginResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            FullName = user.FullName,
            Role = user.Role.ToString()
        });
    }
}
