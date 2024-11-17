using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyTemplate.Application.ApplicationManagement.Common.Constants;
using MyTemplate.Application.ApplicationManagement.Services;
using ClaimTypes = MyTemplate.Application.ApplicationManagement.Common.Constants.ClaimTypes;

namespace MyTemplate.Infrastructure.Services;

public class TokenService: ITokenService
{
    private readonly IHashService _hashService;
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;
    
    public TokenService(IHashService hashService, IConfiguration configuration)
    {
        _hashService = hashService;
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
    }
    public string CreateToken(int userId, DateTime expiresIn, TokenType tokenType)
    {
        var authClaims = new List<Claim>()
        {
            new(ClaimTypes.Id, _hashService.Encode(userId)),
            new(ClaimTypes.TokenType, ((int)tokenType).ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: expiresIn,
            claims: authClaims,
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token, TokenType tokenType)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Token doğrulamasında tolerans süresi
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var validatedToken);

            if (!IsTokenTypeValid(principal, tokenType))
            {
                throw new CustomException(CustomResponseMessages.InvalidToken);
            }

            return principal;
        }
        catch (SecurityTokenException)
        {
            throw new CustomException(CustomResponseMessages.InvalidToken);
        }
        catch (Exception)
        {
            throw new CustomException(CustomResponseMessages.InvalidToken);
        }
    }

    public int GetUserId(string token, TokenType tokenType)
    {
        var principal = ValidateToken(token, tokenType);

        var userId = principal.Claims.First(c => c.Type == ClaimTypes.Id).Value;

        return _hashService.Decode(userId);
    }
    
    private bool IsTokenTypeValid(ClaimsPrincipal principal, TokenType tokenType)
    {
        var tokenTypeClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.TokenType);

        if (tokenTypeClaim == null)
        {
            return false;
        }

        return tokenTypeClaim.Value == ((int)tokenType).ToString();
    }
}