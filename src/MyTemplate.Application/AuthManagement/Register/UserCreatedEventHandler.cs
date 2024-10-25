using System.Text;
using Common.Builders;
using Common.Events;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MyTemplate.Application.ApplicationManagement.Services;
using MyTemplate.Domain.Entities.Identity;
using H = MyTemplate.Application.ApplicationManagement.Helpers.Helper;

namespace MyTemplate.Application.AuthManagement.Register;

internal class UserCreatedEventHandler : NotificationHandlerBase<UserCreatedEvent>
{
    private readonly IMessageService _messageService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHashService _hashService;
    private readonly IConfiguration _configuration;

    public UserCreatedEventHandler(IMessageService messageService, UserManager<ApplicationUser> userManager, IHashService hashService, IConfiguration configuration)
    {
        _messageService = messageService;
        _userManager = userManager;
        _hashService = hashService;
        _configuration = configuration;
    }

    protected override async Task HandleAsync(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var clientAppUrl = H.GetValueFromConfiguration<string>(_configuration, "ClientApp:Url");

        var templateContent = await H.GetHtmlTemplateAsync(cancellationToken, "Templates", "Email", "Register.mjml");

        var confirmUrl = await GenerateConfirmUrlAsync(notification.User, clientAppUrl);

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

   
    private async Task<string> GenerateConfirmUrlAsync(ApplicationUser user, string clientAppUrl)
    {
        string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

        string link = $"{clientAppUrl}/api/auth/validate-mail?userId={_hashService.Encode(user.Id)}&token={encodedConfirmationToken}";

        return link;
    }
}