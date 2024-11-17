using Common.Builders;
using Common.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyTemplate.Application.ApplicationManagement.Services;
using MyTemplate.Domain.Entities.Identity;
using Helper = MyTemplate.Application.ApplicationManagement.Helpers.Helper;

namespace MyTemplate.Application.AuthManagement.Register;

internal class UserCreatedEventHandler : NotificationHandlerBase<UserCreatedEvent>
{
    private readonly IMessageService _messageService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHashService _hashService;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public UserCreatedEventHandler(IMessageService messageService, UserManager<ApplicationUser> userManager, IHashService hashService, IConfiguration configuration, ITokenService tokenService)
    {
        _messageService = messageService;
        _userManager = userManager;
        _hashService = hashService;
        _configuration = configuration;
        _tokenService = tokenService;
    }
    
    protected override async Task HandleAsync(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var clientAppUrl = Helper.GetValueFromConfiguration<string>(_configuration, "ClientApp:Url")!;

        var templateContent = await Helper.GetHtmlTemplateAsync(cancellationToken, "Templates", "Email", "Register.mjml");

        var confirmUrl = GenerateConfirmUrlAsync(notification.User, clientAppUrl);

        var replaceBuilder = new ReplaceBuilder(templateContent)
                                 .Replace("{{confirmUrl}}", confirmUrl)
                                 .Replace("{{companyName}}", clientAppUrl)
                                 ;
        await _messageService.PublisAsync<MailSendEvent>(new()
        {
            Subject = "Email Adresi Doğrulama",
            Body = replaceBuilder.Value,
            To = [notification.User.Email!]
        }, cancellationToken);
    }

   
    private  string GenerateConfirmUrlAsync(ApplicationUser user, string clientAppUrl)
    {
        var token = _tokenService.CreateToken(user.Id, DateTime.Now.AddHours(2));
        
        return  $"{clientAppUrl}/api/auth/validate-mail?token={token}";
    }
}