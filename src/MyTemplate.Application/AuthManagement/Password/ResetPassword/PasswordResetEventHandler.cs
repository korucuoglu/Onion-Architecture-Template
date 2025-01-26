using Common.Builders;
using Common.Constants;
using Common.Events;
using Common.Extensions;
using Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Helper = MyTemplate.Application.ApplicationManagement.Helpers.Helper;

namespace MyTemplate.Application.AuthManagement.Password.ResetPassword;

internal class PasswordResetEventHandler : NotificationHandlerBase<PasswordResetEvent>
{
    private readonly IMessageService _messageService;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public PasswordResetEventHandler(IMessageService messageService,IConfiguration configuration, ITokenService tokenService)
    {
        _messageService = messageService;
        _configuration = configuration;
        _tokenService = tokenService;
    }
    
    protected override async Task HandleAsync(PasswordResetEvent notification, CancellationToken cancellationToken)
    {
        var clientAppUrl = _configuration.GetConfigValue<string>("ClientApp:Url")!;
        
        var templateContent = await Helper.GetHtmlTemplateAsync(cancellationToken, "Templates", "Email", "ResetPassword.mjml");

        var confirmUrl = GenerateConfirmUrl(notification.User, clientAppUrl);

        var replaceBuilder = new ReplaceBuilder(templateContent)
                                 .Replace("{{url}}", confirmUrl)
                                 .Replace("{{companyName}}", clientAppUrl)
                                 ;
        await _messageService.PublisAsync<MailSendEvent>(new()
        {
            Subject = "Parola Sıfırlama Talebi",
            Body = replaceBuilder.Value,
            To = [notification.User.Email!]
        }, cancellationToken);
    }

   
    private  string GenerateConfirmUrl(ApplicationUser user, string clientAppUrl)
    {
        var validateTokenUrl = _configuration.GetConfigValue<string>("ClientApp:ValidateToken")!;
        
        var token = _tokenService.CreateToken(user.Id, DateTime.Now.AddMinutes(15), TokenType.PasswordToken);
        
        var url = string.Format(validateTokenUrl, clientAppUrl, token);

        return url;
    }
}