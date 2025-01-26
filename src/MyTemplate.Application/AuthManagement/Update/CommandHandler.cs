using Microsoft.AspNetCore.Identity;
namespace MyTemplate.Application.AuthManagement.Update;

public class CommandHandler : CommandHandlerBase<Command>
{
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISettingService _settingService;
    private readonly IUserContextAccessor _userContextAccessor;

    public CommandHandler(UserManager<ApplicationUser> userManager, ISettingService settingService, IUserContextAccessor userContextAccessor)
    {
        _userManager = userManager;
        _settingService = settingService;
        _userContextAccessor = userContextAccessor;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var userId = _userContextAccessor.UserId;

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.UserNotFound));
        }

        var isEmailChanged = user.Email!.Equals(request.Email) == false; // email değişti mi?

        user.EmailConfirmed = isEmailChanged ? false : user.EmailConfirmed; // Email'ini değiştirmesi durumunda "false" olur.
        user.Email = request.Email;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errMessage = result.Errors.FirstOrDefault()?.Description ?? "Hata meydana geldi";

            return Result.WithFailure(Error.WithMessage(errMessage));
        }

        var emailConfirmRequired = _settingService.EmailConfirmRequired();

        if (emailConfirmRequired && isEmailChanged)
        {
            // await SendMail();
        }

        return Result.WithSuccess();
    }
}