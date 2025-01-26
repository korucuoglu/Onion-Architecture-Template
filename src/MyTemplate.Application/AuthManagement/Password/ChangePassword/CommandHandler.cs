using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Application.AuthManagement.Password.ChangePassword;

public class CommandHandler : CommandHandlerBase<Command>
{
    private UserManager<ApplicationUser> _userManager;
    private readonly IUserContextAccessor _userContextAccessor;

    public CommandHandler(UserManager<ApplicationUser> userManager, IUserContextAccessor userContextAccessor)
    {
        _userManager = userManager;
        _userContextAccessor = userContextAccessor;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        if (!_userContextAccessor.IsAuthenticated)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.UnAuthenticated));
        }
        
        var userId = _userContextAccessor.UserId;

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
            var errMessage = result.Errors.FirstOrDefault()?.Description ?? "Parola güncellenemedi";

            return Result.WithFailure(Error.WithMessage(errMessage));
        }

        return Result.WithSuccess();
    }
}