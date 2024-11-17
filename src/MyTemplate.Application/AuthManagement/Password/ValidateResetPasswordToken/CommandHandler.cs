using MyTemplate.Application.ApplicationManagement.Common.Constants;
using MyTemplate.Application.ApplicationManagement.Services;

namespace MyTemplate.Application.AuthManagement.ValidateResetPasswordToken;

public class CommandHandler : CommandHandlerBase<Command>
{
    private readonly ITokenService _tokenService;

    public CommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        _tokenService.ValidateToken(request.Token, TokenType.PasswordToken);
        
        return Result.WithSuccess();
    }
}