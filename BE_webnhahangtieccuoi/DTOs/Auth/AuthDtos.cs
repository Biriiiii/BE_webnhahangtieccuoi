namespace BE_webnhahangtieccuoi.DTOs.Auth;

public class LoginRequestDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public string FullName { get; set; } = null!;
    public string Role { get; set; } = null!;
}
