using System.Text;
using Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MyTemplate.Application.ApplicationManagement.Interfaces;
using MyTemplate.Application.AuthManagement;

namespace MyTemplate.Application.AuthManagement.ValidateMail;

public class CommandHandler : CommandHandlerBase<Command>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHashService _hashService;
    private readonly ISettingService _settingService;

    public CommandHandler(UserManager<ApplicationUser> userManager, IHashService hashService, ISettingService settingService)
    {
        _userManager = userManager;
        _hashService = hashService;
        _settingService = settingService;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        if (!_settingService.EmailConfirmRequired())
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }

        var userId = _hashService.Decode(request.UserId);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }

        if (user.EmailConfirmed)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }

        await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return Result.WithSuccess();
    }
}