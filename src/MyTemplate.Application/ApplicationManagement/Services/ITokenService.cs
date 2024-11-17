using System.Security.Claims;

namespace MyTemplate.Application.ApplicationManagement.Services;

public interface ITokenService
{
    public string CreateToken(int userId, DateTime expiresIn);
    public ClaimsPrincipal? ValidateToken(string token);
    public int GetUserId(string token);
}