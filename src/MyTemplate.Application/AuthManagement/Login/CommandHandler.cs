using Common.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Application.AuthManagement.Login;

public class CommandHandler : CommandHandlerBase<Command, Dto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISettingService _settingService;
    private readonly ITokenService _tokenService;

    public CommandHandler(UserManager<ApplicationUser> userManager, ISettingService settingService, ITokenService tokenService)
    {
        _userManager = userManager;
        _settingService = settingService;
        _tokenService = tokenService;
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

        var accessToken = _tokenService.CreateToken(user.Id, DateTime.Now.AddDays(3), TokenType.AccessToken);

        return Result<Dto>.WithSuccess(new()
        {
            AccessToken = accessToken,
        });
    }
}