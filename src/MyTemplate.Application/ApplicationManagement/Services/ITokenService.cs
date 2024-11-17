using System.Security.Claims;
using MyTemplate.Application.ApplicationManagement.Common.Constants;

namespace MyTemplate.Application.ApplicationManagement.Services;

public interface ITokenService
{
    public string CreateToken(int userId, DateTime expiresIn, TokenType tokenType);
    public ClaimsPrincipal? ValidateToken(string token, TokenType tokenType);
    public int GetUserId(string token,  TokenType tokenType);
}