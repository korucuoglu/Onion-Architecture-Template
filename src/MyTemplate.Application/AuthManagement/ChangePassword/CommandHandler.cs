using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Helpers;
using MyTemplate.Application.AuthManagement;

namespace MyTemplate.Application.AuthManagement.ChangePassword;

public class CommandHandler : CommandHandlerBase<Command>
{
    private UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHashService _hashService;

    public CommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IHashService hashService)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _hashService = hashService;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var encodedUserId = Helper.GetUserId(_httpContextAccessor);

        var userId = _hashService.Decode(encodedUserId);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var errMessage = result.Errors.FirstOrDefault()?.Description ?? "Şifre güncellenemedi";

            return Result.WithFailure(Error.WithMessage(errMessage));
        }

        return Result.WithSuccess();
    }
}