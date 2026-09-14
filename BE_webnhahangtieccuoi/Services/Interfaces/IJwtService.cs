using BE_webnhahangtieccuoi.Models.Entities;

namespace BE_webnhahangtieccuoi.Services.Interfaces;

public interface IJwtService
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}
