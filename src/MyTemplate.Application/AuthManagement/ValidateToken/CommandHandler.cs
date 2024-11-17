using Microsoft.AspNetCore.Identity;
using MyTemplate.Application.ApplicationManagement.Common.Constants;
using MyTemplate.Application.ApplicationManagement.Services;
using MyTemplate.Domain.Entities.Identity;

namespace MyTemplate.Application.AuthManagement.ValidateToken;

public class CommandHandler : CommandHandlerBase<Command>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public CommandHandler(UserManager<ApplicationUser> userManager, ISettingService settingService, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.ValidateToken(request.Token);

        if (principal is null)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }
        
        return Result.WithSuccess();
    }
}