using Common.Builders;
using Common.Events;
using Microsoft.Extensions.Configuration;
using MyTemplate.Application.ApplicationManagement.Common.Constants;
using MyTemplate.Application.ApplicationManagement.Services;
using MyTemplate.Domain.Entities.Identity;
using Helper = MyTemplate.Application.ApplicationManagement.Helpers.Helper;

namespace MyTemplate.Application.AuthManagement.Register;

internal class UserCreatedEventHandler : NotificationHandlerBase<UserCreatedEvent>
{
    private readonly IMessageService _messageService;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public UserCreatedEventHandler(IMessageService messageService,IConfiguration configuration, ITokenService tokenService)
    {
        _messageService = messageService;
        _configuration = configuration;
        _tokenService = tokenService;
    }
    
    protected override async Task HandleAsync(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var clientAppUrl = Helper.GetValueFromConfiguration<string>(_configuration, "ClientApp:Url")!;
        
        var confirmUrl = GenerateConfirmUrl(notification.User, clientAppUrl);
        
        var templateContent = await Helper.GetHtmlTemplateAsync(cancellationToken, "Templates", "Email", "Register.mjml");

        var replaceBuilder = new ReplaceBuilder(templateContent)
                                 .Replace("{{url}}", confirmUrl)
                                 .Replace("{{companyName}}", clientAppUrl)
                                 ;
        await _messageService.PublisAsync<MailSendEvent>(new()
        {
            Subject = "Email Adresi Doğrulama",
            Body = replaceBuilder.Value,
            To = [notification.User.Email!]
        }, cancellationToken);
    }

   
    private  string GenerateConfirmUrl(ApplicationUser user, string clientAppUrl)
    {
        var validateMailUrl = Helper.GetValueFromConfiguration<string>(_configuration, "ClientApp:ValidateMail")!;
        
        var token = _tokenService.CreateToken(user.Id, DateTime.Now.AddMinutes(15), TokenType.MailToken);
        
        var url = string.Format(validateMailUrl, clientAppUrl, token);

        return url;
    }
}