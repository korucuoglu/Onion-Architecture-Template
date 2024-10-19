using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MyTemplate.Application.ApplicationManagement.Interfaces;

namespace MyTemplate.Application.UserManagement.Register;

public class CommandHandler : CommandHandlerBase<Command>
{
    private UserManager<ApplicationUser> _userManager;
    private readonly ISettingService _settingService;

    public CommandHandler(UserManager<ApplicationUser> userManager, ISettingService settingService)
    {
        _userManager = userManager;
        _settingService = settingService;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser()
        {
            UserName = request.Username,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errMessage = result.Errors.Select(x => x.Description).FirstOrDefault() ?? "Kullanıcı kaydı yapılamadı.";

            return Result.WithFailure(Error.WithMessage(errMessage));
        }

        var emailConfirmRequired = _settingService.EmailConfirmRequired();

        if (emailConfirmRequired)
        {
            string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

            // email.Send()
        }

        return Result.WithSuccess();
    }
}