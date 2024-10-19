using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Helpers;
using MyTemplate.Application.ApplicationManagement.Interfaces;

namespace MyTemplate.Application.UserManagement.Update;

public class CommandHandler : CommandHandlerBase<Command>
{
    private UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHashService _hashService;
    private readonly ISettingService _settingService;

    public CommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IHashService hashService, ISettingService settingService)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _hashService = hashService;
        _settingService = settingService;
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