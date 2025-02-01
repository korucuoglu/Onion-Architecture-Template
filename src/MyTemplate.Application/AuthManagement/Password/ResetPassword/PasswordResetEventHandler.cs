using Common.Builders;
using Common.Constants;
using Common.Extensions;
using Common.Interfaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace MyTemplate.Application.AuthManagement.Password.ResetPassword;

internal class PasswordResetEventHandler : NotificationHandlerBase<PasswordResetEvent>
{
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly IMailService _mailService;

    public PasswordResetEventHandler(IConfiguration configuration, ITokenService tokenService, IMailService mailService)
    {
        _configuration = configuration;
        _tokenService = tokenService;
        _mailService = mailService;
    }
    
    protected override async Task HandleAsync(PasswordResetEvent notification, CancellationToken cancellationToken)
    {
        var clientAppUrl = _configuration.GetConfigValue<string>("ClientApp:Url")!;

        var templateContent = await GetTemplateContentAsync(cancellationToken);

        var confirmUrl = GenerateConfirmUrl(notification.User, clientAppUrl);

        var replaceBuilder = new ReplaceBuilder(templateContent)
                                 .Replace("{{url}}", confirmUrl)
                                 .Replace("{{companyName}}", clientAppUrl)
                                 ;
        await _mailService.SendAsync(new()
        {
            Subject = "Parola Sıfırlama Talebi",
            Body = replaceBuilder.Value,
            To = [notification.User.Email!]
        }, cancellationToken);
    }
    
    private async Task<string> GetTemplateContentAsync(CancellationToken cancellationToken)
    {
        var assembly = typeof(TemplateExtensions).Assembly;
        
        const string resourceName = "Common.Templates.Email.ResetPassword.mjml";

        var htmlContent = await assembly.GetHtmlTemplateAsync(resourceName, cancellationToken);
        
        return htmlContent;
    }

   
    private  string GenerateConfirmUrl(ApplicationUser user, string clientAppUrl)
    {
        var validateTokenUrl = _configuration.GetConfigValue<string>("ClientApp:ValidateToken")!;
        
        var token = _tokenService.CreateToken(user.Id, DateTime.Now.AddMinutes(15), TokenType.PasswordToken);
        
        var url = string.Format(validateTokenUrl, clientAppUrl, token);

        return url;
    }
}