using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Application.AuthManagement.Password.ResetPassword;

public class CommandHandler: CommandHandlerBase<Command>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    
    public CommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    protected override async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            /*
             * Böyle bir mail adresi yoksa bile olmadığını söylemek mantıklı mı? 
             * Kullanıcı bu sistemi kötüye kullanabilir. Brute-Force yapabilir. 
             * UI'da her zaman "sistemde kayıtlı mail adresiniz varsa ona mail gönderildi" şeklinde bir mesaj gönderilebilir.
             */ 
            return Result.WithSuccess(); 
        }

        _= _mediator.Publish<PasswordResetEvent>(new()
        {
            User = user,
        }, cancellationToken);

        return Result.WithSuccess();
    }
}