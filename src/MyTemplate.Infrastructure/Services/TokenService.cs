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


    public string CreateToken(int userId, DateTime expiresIn)
    {
        var authClaims = new List<Claim>()
        {
            new(ClaimTypes.Id, _hashService.Encode(userId)),
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

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Token doğrulamasında tolerans süresi
            }, out var validatedToken);

            return principal;
        }
        catch
        {
            return null; // Token geçersizse null döndür
        }
    }

    public int GetUserId(string token)
    {
        var princilal = ValidateToken(token);

        if (princilal is null)
        {
            throw new CustomException(CustomResponseMessages.InvalidToken);
        }
        
        var userId = princilal.Claims.First(c => c.Type == ClaimTypes.Id).Value;

        return _hashService.Decode(userId);
    }
}