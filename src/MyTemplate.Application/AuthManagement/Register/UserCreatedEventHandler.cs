using Common.Builders;
using Common.Constants;
using Common.Extensions;
using Common.Interfaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace MyTemplate.Application.AuthManagement.Register;

internal class UserCreatedEventHandler : NotificationHandlerBase<UserCreatedEvent>
{
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private readonly IMailService _mailService;

    public UserCreatedEventHandler(IConfiguration configuration, ITokenService tokenService, IMailService mailService)
    {
        _configuration = configuration;
        _tokenService = tokenService;
        _mailService = mailService;
    }
    
    protected override async Task HandleAsync(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var clientAppUrl = _configuration.GetConfigValue<string>("ClientApp:Url")!;
        
        var confirmUrl = GenerateConfirmUrl(notification.User, clientAppUrl);

        var templateContent = await GetTemplateContentAsync(cancellationToken);

        var replaceBuilder = new ReplaceBuilder(templateContent)
                                 .Replace("{{url}}", confirmUrl)
                                 .Replace("{{companyName}}", clientAppUrl)
                                 ;
        await _mailService.SendAsync(new()
        {
            Subject = "Email Adresi Doğrulama",
            Body = replaceBuilder.Value,
            To = [notification.User.Email!]
        }, cancellationToken);
    }
    
    private async Task<string> GetTemplateContentAsync(CancellationToken cancellationToken)
    {
        var assembly = typeof(TemplateExtensions).Assembly;
        
        const string resourceName = "Common.Templates.Email.Register.mjml";

        var htmlContent = await assembly.GetHtmlTemplateAsync(resourceName, cancellationToken);
        
        return htmlContent;
    }
   
    private  string GenerateConfirmUrl(ApplicationUser user, string clientAppUrl)
    {
        var validateMailUrl = _configuration.GetConfigValue<string>("ClientApp:ValidateMail")!;
        
        var token = _tokenService.CreateToken(user.Id, DateTime.Now.AddMinutes(15), TokenType.MailToken);
        
        var url = string.Format(validateMailUrl, clientAppUrl, token);

        return url;
    }
    
}