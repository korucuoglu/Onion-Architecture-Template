using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyTemplate.Application.ApplicationManagement.Interfaces;
using MyTemplate.Application.AuthManagement;

namespace MyTemplate.Application.AuthManagement.Login;

public class CommandHandler : CommandHandlerBase<Command, Dto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHashService _hashService;
    private readonly ISettingService _settingService;

    public CommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHashService hashService, ISettingService settingService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _hashService = hashService;
        _settingService = settingService;
    }

    protected override async Task<Result<Dto>> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            return Result<Dto>.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<Dto>.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        var emailConfirmRequired = _settingService.EmailConfirmRequired();

        if (emailConfirmRequired && user.EmailConfirmed == false)
        {
            return Result<Dto>.WithFailure(Error.WithMessage(CustomResponseMessages.EmailNotConfirmed));
        }

        return Result<Dto>.WithSuccess(new()
        {
            AccessToken = CreateToken(user),
        });
    }

    private string CreateToken(ApplicationUser user)
    {
        var authClaims = new List<Claim>()
        {
            new("id", _hashService.Encode(user.Id)),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}