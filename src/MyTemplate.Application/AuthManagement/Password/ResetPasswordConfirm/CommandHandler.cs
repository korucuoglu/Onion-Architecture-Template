using Common.Constants;
using Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Application.AuthManagement.Password.ResetPasswordConfirm;

public class CommandHandler : CommandHandlerBase<Command>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public CommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var userId = _tokenService.GetUserId(request.Token, TokenType.PasswordToken);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result.WithFailure(Error.WithMessage(CustomResponseMessages.InvalidUrl));
        }
    
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

        if (!result.Succeeded)
        {
            var errMessage = result.Errors.FirstOrDefault()?.Description ?? CustomResponseMessages.InvalidUrl;
            
            return Result.WithFailure(Error.WithMessage(errMessage));
        }
        
        return Result.WithSuccess();
    }
}