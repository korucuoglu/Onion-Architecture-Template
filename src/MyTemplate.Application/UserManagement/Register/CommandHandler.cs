using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace MyTemplate.Application.UserManagement.Register;
public class CommandHandler : CommandHandlerBase<Command>
{
    private UserManager<ApplicationUser> _userManager;
    private IUnitOfWork _unitOfWork;
    public CommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
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

        await using var unitOfWork = _unitOfWork.EF;

        var settingRepository = unitOfWork.GetRepository<Setting, int>();

        var setting = await settingRepository.FirstOrDefaultAsync(x => x.Key == "EmailConfirmRequired");

        if (setting is null)
        {
            return Result.WithSuccess();
        }

        var settingValue = Helper.GetSettingValue(setting);

        if (settingValue is bool emailConfirmRequired && emailConfirmRequired)
        {
            string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));
           
            // email.Send()
        }

        return Result.WithSuccess();
    }
}
