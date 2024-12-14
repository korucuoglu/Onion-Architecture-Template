
namespace MyTemplate.Application.AuthManagement.Password.ValidateResetPasswordToken;

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