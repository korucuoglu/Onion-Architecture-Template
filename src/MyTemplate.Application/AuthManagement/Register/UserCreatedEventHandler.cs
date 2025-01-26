using Common.Builders;
using Common.Constants;
using Common.Events;
using Common.Extensions;
using Common.Interfaces;
using Microsoft.Extensions.Configuration;

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
        var clientAppUrl = _configuration.GetConfigValue<string>("ClientApp:Url")!;
        
        var confirmUrl = GenerateConfirmUrl(notification.User, clientAppUrl);

        var templateContent = await GetTemplateContentAsync(cancellationToken);

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