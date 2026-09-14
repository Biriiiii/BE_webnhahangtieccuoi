using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace BE_webnhahangtieccuoi.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public (string token, DateTime expiresAt) GenerateToken(User user)
    {
        var secretKey = _config["Jwt:Key"];
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException(
                "Jwt:Key chưa được cấu hình trong appsettings.json"
            );

        if (string.IsNullOrWhiteSpace(issuer))
            throw new InvalidOperationException(
                "Jwt:Issuer chưa được cấu hình trong appsettings.json"
            );

        if (string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException(
                "Jwt:Audience chưa được cấu hình trong appsettings.json"
            );

        var expireMinutes = 120;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("fullName", user.FullName)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var expiresAt = DateTime.UtcNow.AddMinutes(expireMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        return (
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAt
        );
    }
}