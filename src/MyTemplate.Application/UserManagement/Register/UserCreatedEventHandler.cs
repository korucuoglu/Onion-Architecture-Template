using System.Text;
using Common.Interfaces;
using Common.Interfaces.Services;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace MyTemplate.Application.UserManagement.Register;

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
        var confirmUrl = await GenerateConfirmUrlAsync(notification.User);

        await _messageService.PublisAsync<MailSendEvent>(new()
        {
            Subject = "Email Adresi Doğrulama",
            Body = confirmUrl,
            To = [notification.User.Email!]
        }, cancellationToken);
    }

    private async Task<string> GenerateConfirmUrlAsync(ApplicationUser user)
    {
        var baseUrl = _configuration.GetValue<string>("ClientApp:Url") ?? throw new Exception("BaseUrl boş olamaz");

        string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

        string link = $"{baseUrl}/api/auth/validate-mail?userId={_hashService.Encode(user.Id)}&token={encodedConfirmationToken}";

        return link;
    }
}