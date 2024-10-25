using Common.Events;
using MassTransit;
using MyTemplate.WorkerService.Services;

namespace MyTemplate.WorkerService.Consumers;

public class EmailMessageConsumer : IConsumer<MailSendEvent>
{
    private readonly MailService _mailService;
    private readonly ILogger<EmailMessageConsumer> _logger;

    public EmailMessageConsumer(MailService mailService, ILogger<EmailMessageConsumer> logger)
    {
        _mailService = mailService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MailSendEvent> context)
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