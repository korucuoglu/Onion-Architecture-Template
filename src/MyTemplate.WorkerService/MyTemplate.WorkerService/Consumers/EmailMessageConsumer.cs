using Common.Interfaces;
using Common.Models;
using Common.Services;
using MassTransit;

namespace MyTemplate.WorkerService.Consumers;

public class EmailMessageConsumer : IConsumer<MailSendInput>
{
    private readonly IMailService _mailService;
    private readonly ILogger<EmailMessageConsumer> _logger;

    public EmailMessageConsumer(IMailService mailService, ILogger<EmailMessageConsumer> logger)
    {
        _mailService = mailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MailSendInput> context)
    {
        var emailMessage = context.Message;

        try
        {
            await _mailService.SendAsync(emailMessage);

            _logger.LogInformation("Email sent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending email");
        }
    }
}