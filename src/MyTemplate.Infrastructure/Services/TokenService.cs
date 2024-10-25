using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyTemplate.Application.ApplicationManagement.Services;

namespace MyTemplate.Infrastructure.Services;

public class TokenService: ITokenService
{
    private readonly IHashService _hashService;
    private readonly IConfiguration _configuration;
    
    public TokenService(IHashService hashService, IConfiguration configuration)
    {
        _hashService = hashService;
        _configuration = configuration;
    }


    public string CreateToken(int userId, DateTime expiresIn)
    {
        var authClaims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, _hashService.Encode(userId)),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: expiresIn,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}